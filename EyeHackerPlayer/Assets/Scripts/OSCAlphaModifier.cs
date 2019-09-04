﻿using System.Collections;
using System.Collections.Generic;
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

    public Color color = new Color (0.0f, 165.0f / 255.0f, 255.0f / 255.0f, 1.0f);
    [SerializeField] private uOscServer server;

    // Start is called before the first frame update
    void Start ()
    {
        for (int i = 0; i < spheres.Length; i++)
        {
            alphas[i] = 0.0f;
        }

        isSelected = false;
        center = new Vector3 (0.0f, 0.0f, 0.0f);

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
                selectedBall = spheres[14]; // select a sphere
                isSelected = true;
            }
            // int num = System.Convert.ToInt32 (message.values[1]) - 1;
            // if (num == -1) return;
            float val = System.Convert.ToSingle (message.values[0]);
            // alphas[num] = val;
            selectedBall.GetComponent<Renderer> ().material.color = new Vector4 (color.r, color.g, color.b, val);
        }
    }
}