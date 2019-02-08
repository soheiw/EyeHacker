using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class AdjustThresholdByHMDRotation : MonoBehaviour
{
    public float magnitude;
    private Vector3 direction;
    uOscServer server;
    [SerializeField] string address;
    [SerializeField] bool sync = true;

    // Use this for initialization
    void Start ()
    {
        direction = new Vector3 (0, 0, 0);
        
        server = FindObjectOfType<uOscServer> ();
        if (!server) return;
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == address)
        {
            if (!sync) return;

            direction.x = (float) message.values[0];
            direction.y = (float) message.values[1];
            direction.z = (float) message.values[2];

            magnitude = direction.magnitude;
        }
    }
}