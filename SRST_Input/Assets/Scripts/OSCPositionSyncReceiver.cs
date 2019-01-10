using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCPositionSyncReceiver : MonoBehaviour
{
    uOscServer server;
    [SerializeField] string address;
    [SerializeField] bool sync = true;
    [SerializeField] bool isLocal = false;

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
            var position = new Vector3 (0, 0, 0);
            var rotation = new Vector3 (0, 0, 0);

            position.x = (float) message.values[0];
            position.y = (float) message.values[1];
            position.z = (float) message.values[2];

            // Billboardしたいのでrotationは捨てる
            // rotation.x = (float) message.values[3];
            // rotation.y = (float) message.values[4];
            // rotation.z = (float) message.values[5];

            if (!sync) return;

            if (isLocal)
            {
                transform.localPosition = position;
                // transform.localRotation = Quaternion.Euler (rotation);
            }
            else
            {
                transform.position = position;
                // transform.rotation = Quaternion.Euler (rotation);
            }
        }
    }
}