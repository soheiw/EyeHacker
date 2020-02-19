using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uOSC;
using UnityEngine;
using ViveSR.anipal.Eye;

public class OSCAlphaModifier : MonoBehaviour
{
    public GameObject[] spheres;
    public float[] alphas;

    public bool isSelected;
    public SRanipal_GazeRaySample gazeRaySample;
    public GameObject selectedBall;

    public Vector3 center;
    public Color color = new Color (50.0f / 255.0f, 50.0f / 255.0f, 50.0f / 255.0f, 1.0f);

    public OSCBallInfoSender infoSender;

    public float min = 0.0f;
    public float max = 10.0f;

    private float innerRadius;
    private float outerRadius;
    private bool setBallPosleft;

    [SerializeField] float showAlpha = 0.0f;

    [SerializeField] private uOscServer server;

    // Start is called before the first frame update
    void Start ()
    {

        isSelected = false;
        center = new Vector3 (0.0f, 0.0f, 0.0f);
        for (int i = 0; i < spheres.Length; i++)
        {
            alphas[i] = 0.0f;
        }

        if (!server)
        {
            UnityEngine.Debug.Log ("uOSCserver is not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/alpha")
        {
            if (!isSelected)
            {
                center = gazeRaySample.gazePosition;

                if (min * max > 0)
                {
                    innerRadius = Mathf.Min (Mathf.Abs (min), Mathf.Abs (max));
                    outerRadius = Mathf.Max (Mathf.Abs (min), Mathf.Abs (max));
                    setBallPosleft = (min >= 0.0f);
                }
                else
                {
                    innerRadius = 0.0f;
                    outerRadius = min != 0.0f ? Mathf.Abs (min) : Mathf.Abs (max);
                    // setBallPosleft = true;
                }

                Collider[] hitColliders = Physics.OverlapSphere (center, calculateCollisionRadius (outerRadius));

                if (hitColliders.Length == 0)
                {
                    Debug.Log ("No Collision: center: " + center);
                    return;
                }

                if (hitColliders.Length == 1 && hitColliders[0].gameObject.name == "Collider")
                {
                    Debug.Log ("Collider Collision Only: center: " + center);
                    return;
                }

                // shuffle
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    Collider tmp = hitColliders[i];
                    int randomIndex = Random.Range (i, hitColliders.Length);
                    hitColliders[i] = hitColliders[randomIndex];
                    hitColliders[randomIndex] = tmp;
                }

                // int count = 0;
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (!hitColliders[i].gameObject.CompareTag ("changedBall"))
                    {
                        continue;
                    }

                    if (innerRadius == 0.0f)
                    {
                        if (Vector3.Distance (hitColliders[i].gameObject.transform.position, center) > calculateCollisionRadius (outerRadius))
                        {
                            continue;
                        }
                        selectedBall = hitColliders[i].gameObject;
                        color = selectedBall.GetComponent<Renderer> ().material.color;
                        // count++;
                        break;
                    }
                    else
                    {
                        if (Vector3.Distance (hitColliders[i].gameObject.transform.position, center) <= calculateCollisionRadius (innerRadius) || Vector3.Distance (hitColliders[i].gameObject.transform.position, center) > calculateCollisionRadius (outerRadius))
                        {
                            continue;
                        }
                        if (setBallPosleft)
                        {
                            if (hitColliders[i].gameObject.transform.position.x - center.x >= 0.0f)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (hitColliders[i].gameObject.transform.position.x - center.x <= 0.0f)
                            {
                                continue;
                            }
                        }
                        {
                            selectedBall = hitColliders[i].gameObject;
                            color = selectedBall.GetComponent<Renderer> ().material.color;
                            // count++;
                            break;
                        }
                    }
                }
                // Debug.Log ("count: " + count + ", selected: " + selectedBall.name);
                if (selectedBall != null)
                {
                    infoSender.SendInfo ();
                    isSelected = true;
                }
                else
                {
                    Debug.Log ("Nothing is selected.");
                }
            }

            float val = System.Convert.ToSingle (message.values[0]);
            // alphas[num] = val;
            int num = System.Convert.ToInt32 (message.values[1]) - 1;
            if (num == -1) // do not change
            {
                selectedBall = null;
                return;
            }

            if (selectedBall != null)
            {
                selectedBall.GetComponent<Renderer> ().material.color = new Vector4 (color.r, color.g, color.b, val);
                showAlpha = val;
            }
            else
            {
                isSelected = false;
            }
        }
    }

    // cosine formula
    float calculateCollisionRadius (float degree)
    {
        float radius = 9.0f;
        return Mathf.Sqrt (radius * radius + radius * radius - 2 * radius * radius * Mathf.Cos (degree * Mathf.Deg2Rad));
    }
}