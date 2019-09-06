using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateBallPos : MonoBehaviour
{
    public List<List<Vector3>> positionDistributions;

    public Transform staticBalls;
    public Transform changedBalls;

    public GameObject ball;

    // Start is called before the first frame update
    void Awake ()
    {
        // UnityEngine.Random.InitState (42);
        positionDistributions = new List<List<Vector3>> ();
        for (int k = 0; k < 144; k++)
        {
            List<Vector3> distribution = new List<Vector3> ();
            for (int j = 0; j < 180; j++)
            {
                float phi = (1.0f / 180.0f) * j * (2.0f * Mathf.PI);
                float theta = UnityEngine.Random.Range (-Mathf.PI / 18.0f, Mathf.PI / 18.0f);
                Vector3 pos = new Vector3 (9.0f * Mathf.Cos (theta) * Mathf.Cos (phi), 9.0f * Mathf.Sin (theta), 9.0f * Mathf.Cos (theta) * Mathf.Sin (phi));
                distribution.Add (pos);
            }
            distribution = distribution.OrderBy (i => Guid.NewGuid ()).ToList ();
            positionDistributions.Add (distribution);
        }
        positionDistributions = positionDistributions.OrderBy (i => Guid.NewGuid ()).ToList ();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    public void SetBallPositions (int number)
    {
        List<Vector3> positions = positionDistributions[number];
        for (int i = 0; i < 40; i++)
        {
            GameObject g = Instantiate (ball, positions[i], Quaternion.identity);
            g.transform.parent = staticBalls;

        }
        for (int j = 40; j < 180; j++)
        {
            GameObject h = Instantiate (ball, positions[j], Quaternion.identity);
            Color col = h.GetComponent<Renderer> ().material.color;
            h.GetComponent<Renderer> ().material.color = new Vector4 (col.r, col.g, col.b, 0.0f);
            h.tag = "changedBall";
            h.transform.parent = changedBalls;
        }
    }

    public void DestroyAll ()
    {
        foreach (Transform n in staticBalls)
        {
            GameObject.Destroy (n.gameObject);
        }

        foreach (Transform m in changedBalls)
        {
            GameObject.Destroy (m.gameObject);
        }
    }
}