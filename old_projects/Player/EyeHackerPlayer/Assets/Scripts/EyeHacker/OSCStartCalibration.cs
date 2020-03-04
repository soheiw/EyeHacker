using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using ViveSR.anipal.Eye;

public class OSCStartCalibration : MonoBehaviour
{
    [SerializeField] private uOscServer server;

    // Start is called before the first frame update
    void Start ()
    {
        if (!server)
        {
            UnityEngine.Debug.Log ("uOSCserver is not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.C))
        {
            SRanipal_Eye.LaunchEyeCalibration ();
        }
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/startcalibration")
        {
            SRanipal_Eye.LaunchEyeCalibration ();
        }
    }
}