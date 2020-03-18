using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCGazeCoordSender : MonoBehaviour
{
    uOscClient client;
    [SerializeField] RayManager rayManager;
    [SerializeField] DetermineGazeByHMD determineGaze;
    [SerializeField] string address;
    [SerializeField] bool sync = true;

    // Use this for initialization
    void Start ()
    {
        client = FindObjectOfType<uOscClient> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!sync || client == null) return;
        if (rayManager.enabled)
        {
            client.Send (address, rayManager.gazeCoord.x, rayManager.gazeCoord.y);
        }
        else if (determineGaze.enabled)
        {
            client.Send (address, determineGaze.gazeCoord.x, determineGaze.gazeCoord.y);
        }
    }
}