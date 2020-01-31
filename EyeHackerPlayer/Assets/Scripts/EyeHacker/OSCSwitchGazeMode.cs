using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using ViveSR.anipal.Eye;

[RequireComponent (typeof (SRanipal_GazeRaySample))]
[RequireComponent (typeof (DetermineGazeByHMD))]
public class OSCSwitchGazeMode : MonoBehaviour
{
    private SRanipal_GazeRaySample gazeRaySample;
    private DetermineGazeByHMD determineGazeByHMD;

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

        gazeRaySample = GetComponent<SRanipal_GazeRaySample> ();
        determineGazeByHMD = GetComponent<DetermineGazeByHMD> ();
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/switchgaze")
        {
            var state = message.values[0].ToString ();
            if (state != "0")
            {
                if (!gazeRaySample.enabled) return;
                gazeRaySample.enabled = false;
                determineGazeByHMD.enabled = true;
            }
            else
            {
                if (gazeRaySample.enabled) return;
                gazeRaySample.enabled = true;
                determineGazeByHMD.enabled = false;
            }
        }
    }
}