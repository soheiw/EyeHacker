using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;

public class ShowText : MonoBehaviour
{
    public SRanipal_GazeRaySample gazeRaySample;
    Text text;

    // Start is called before the first frame update
    void Start ()
    {
        text = this.GetComponent<Text> ();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 gazeDirection = gazeRaySample.GetGazeDirection ();
        String str = String.Format ("GazeDirectionSender\nx: {0:0.0000}\ny: {1:0.0000}\nz: {2:0.0000}", gazeDirection.x, gazeDirection.y, gazeDirection.z);
        text.text = str;
    }
}