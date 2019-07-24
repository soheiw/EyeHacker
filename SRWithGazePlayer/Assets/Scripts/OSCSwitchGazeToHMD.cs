using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCSwitchGazeToHMD : MonoBehaviour
{
    private RayManager rayManager;
    private DetermineGazeByHMD determineGazeByHMD;

    [SerializeField] private uOscServer server;

    // Use this for initialization
    void Start ()
    {
        if (!server)
        {
            UnityEngine.Debug.Log ("OSCserver not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);

        rayManager = GetComponent<RayManager> ();
        determineGazeByHMD = GetComponent<DetermineGazeByHMD> ();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/switchgaze")
        {
            var state = message.values[0].ToString ();
            if (state != "0")
            {
                if (!rayManager.enabled) return;
                rayManager.enabled = false;
                determineGazeByHMD.enabled = true;
            }
            else
            {
                if (rayManager.enabled) return;
                rayManager.enabled = true;
                determineGazeByHMD.enabled = false;
            }
        }
    }
}