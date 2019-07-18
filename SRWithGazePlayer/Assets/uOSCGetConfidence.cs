using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class uOSCGetConfidence : MonoBehaviour
{
    public PupilSettings setting;
    public uOscServer server;

    // Use this for initialization
    void Start ()
    {
        if (!server)
        {
            UnityEngine.Debug.Log ("OSCserver not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);
    }

    // Update is called once per frame
    void Update ()
    {
        Debug.Log (setting.connection.confidenceThreshold);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/confidencethr")
        {
            float thr = System.Convert.ToSingle (message.values[0]);
            setting.connection.confidenceThreshold = thr;
        }
    }
}