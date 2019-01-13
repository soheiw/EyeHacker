using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCTest : MonoBehaviour
{
    public string videoName;
    private uOscClient client;

    // Use this for initialization
    void Start ()
    {
        client = GetComponent<uOscClient> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.R))
        {
            client.Send ("/ndiserver/Recorder1/startrecording", videoName);
        }

        if (Input.GetKeyDown (KeyCode.T))
        {
            client.Send ("/ndiserver/Recorder1/stoprecording");
        }
    }
}