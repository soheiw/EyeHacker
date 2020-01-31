using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCShowRay : MonoBehaviour
{
    public LineRenderer lineRenderer;
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

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/viewray")
        {
            var state = message.values[0].ToString ();
            if (state == "0")
            {
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }
}