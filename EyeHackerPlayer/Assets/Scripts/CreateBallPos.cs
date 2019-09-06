﻿using System;
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
        UnityEngine.Random.InitState (42);
        positionDistributions = new List<List<Vector3>> ();
        for (int k = 0; k < 144; k++)
        {
            List<Vector3> distribution = new List<Vector3> ();
            for (int j = 0; j < 90; j++)
            {
                float phi = (1.0f / 90.0f) * j * (2.0f * Mathf.PI);
                float theta = UnityEngine.Random.Range (-Mathf.PI / 18.0f, Mathf.PI / 18.0f);
                Vector3 pos = new Vector3 (9.0f * Mathf.Cos (theta) * Mathf.Cos (phi), 9.0f * Mathf.Sin (theta), 9.0f * Mathf.Cos (theta) * Mathf.Sin (phi));
                distribution.Add (pos);
            }
            // distribution = distribution.OrderBy (i => Guid.NewGuid ()).ToList ();
            positionDistributions.Add (distribution);
        }
        //  positionDistributions = positionDistributions.OrderBy (i => Guid.NewGuid ()).ToList ();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    public void SetBallPositions (int number)
    {
        List<Vector3> positions = positionDistributions[number];

        int areaSize = 10;
        int transparentRatio = 6;

        for (int areaCount = 0; areaCount < areaSize; areaCount++)
        {
            List<Vector3> positionsInArea = new List<Vector3> ();
            for (int k = 0; k < (90 / areaSize); k++)
            {
                positionsInArea.Add (positions[k + areaCount * (90 / areaSize)]);
            }
            positionsInArea = positionsInArea.OrderBy (i => Guid.NewGuid ()).ToList ();
            for (int l = 0; l < transparentRatio; l++)
            {
                GameObject g = Instantiate (ball, positionsInArea[l], Quaternion.identity);
                g.transform.parent = staticBalls;

            }
            for (int m = transparentRatio; m < (90 / areaSize); m++)
            {
                GameObject h = Instantiate (ball, positionsInArea[m], Quaternion.identity);
                Color col = h.GetComponent<Renderer> ().material.color;
                h.GetComponent<Renderer> ().material.color = new Vector4 (col.r, col.g, col.b, 0.0f);
                h.tag = "changedBall";
                h.transform.parent = changedBalls;
            }
            positionsInArea.Clear ();
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