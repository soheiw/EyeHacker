using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using ViveSR.anipal.Eye;

public class DirectionSender : MonoBehaviour
{
    [SerializeField] private uOscClient client;
    [SerializeField] private string address;

    public SRanipal_GazeRaySample gazeRaySample;
    Vector3 gazeDirection;

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
        gazeDirection = gazeRaySample.GetGazeDirection ();
        client.Send (address, gazeDirection.x, gazeDirection.y, gazeDirection.z);
    }
}