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
                // radius = 10 deg, r^2 = 9^2 + 9^2 - 2 * 9 * 9 * cos(10 deg), r = 1.569
                Collider[] hitColliders = Physics.OverlapSphere (center, 1.57f);

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
                    if (hitColliders[i].gameObject.tag == "changedBall")
                    {
                        selectedBall = hitColliders[i].gameObject;
                        color = selectedBall.GetComponent<Renderer> ().material.color;
                        // count++;
                        break;
                    }
                }
                // Debug.Log ("count: " + count + ", selected: " + selectedBall.name);
                if (selectedBall != null)
                {
                    isSelected = true;
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
            }
            else
            {
                isSelected = false;
            }
        }
    }
}