using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using ViveSR.anipal.Eye;

public class OSCGazeCoordSender : MonoBehaviour
{
    [SerializeField] private uOscClient client;
    [SerializeField] private string address;

    [SerializeField] private SRanipal_GazeRaySample gazeRaySample;
    [SerializeField] private DetermineGazeByHMD determineGazeByHMD;

    // Start is called before the first frame update
    void Start ()
    {
        if (!client)
        {
            UnityEngine.Debug.Log ("uOSCclient is not set");
            return;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (gazeRaySample.enabled)
        {
            client.Send (address, gazeRaySample.gazeCoord.x, gazeRaySample.gazeCoord.y);
        }
        else if(determineGazeByHMD.enabled)
        {
            client.Send (address, determineGazeByHMD.gazeCoord.x, determineGazeByHMD.gazeCoord.y);
        }
    }
}