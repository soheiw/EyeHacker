using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCSwitchGazeToHMD : MonoBehaviour
{
    private RayManager rayManager;
    private DetermineGazeByHMD determineGazeByHMD;

    public string playerName;

    [SerializeField] private uOscServer server;

    // Use this for initialization
    void Start ()
    {
        server = FindObjectOfType<uOscServer> ();
        if (!server) return;
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
        if (message.address == "/player/" + playerName + "/switchgaze")
        {
            var state = (float) message.values[0];
            if (state != 0)
            {
                rayManager.enabled = false;
                determineGazeByHMD.enabled = true;
            }
            else
            {
                rayManager.enabled = true;
                determineGazeByHMD.enabled = false;
            }
        }
    }
}