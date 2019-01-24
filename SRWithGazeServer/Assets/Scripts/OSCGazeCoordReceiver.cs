using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCGazeCoordReceiver : MonoBehaviour
{
    uOscServer server;
    [SerializeField] string address;
    [SerializeField] bool sync = true;

    // Use this for initialization
    void Start ()
    {
        server = FindObjectOfType<uOscServer> ();
        if (!server) return;
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == address)
        {
            var gazePoint = new Vector2 (0, 0);
            // var rotation = new Vector3 (0, 0, 0);

            gazePoint.x = (float) message.values[0];
            gazePoint.y = (float) message.values[1];
            Debug.Log (gazePoint);

            if (!sync) return;

            // not implemented
        }
    }
}