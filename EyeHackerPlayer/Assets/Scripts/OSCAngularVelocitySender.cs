using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.XR;

public class OSCAngularVelocitySender : MonoBehaviour
{
    [SerializeField] private uOscClient client;
    [SerializeField] private string address;

    private Vector3 velocity;

    private Vector3 prevDir;
    private Vector3 nextDir;

    private float thr = 300.0f;

    // Start is called before the first frame update
    void Start ()
    {
        if (!client)
        {
            UnityEngine.Debug.Log ("uOSCclient is not set");
            return;
        }
        velocity = new Vector3 (0.0f, 0.0f, 0.0f);
        Quaternion dir_p = InputTracking.GetLocalRotation (XRNode.Head);
        prevDir = dir_p.eulerAngles;
    }

    // Update is called once per frame
    void Update ()
    {
        Quaternion dir_q = InputTracking.GetLocalRotation (XRNode.Head);
        nextDir = dir_q.eulerAngles;

        velocity = nextDir - prevDir;

        if (Mathf.Abs (velocity.x) > thr)
        {
            velocity.x += (velocity.x > 0) ? -360.0f : 360.0f;
        }
        if (Mathf.Abs (velocity.y) > thr)
        {
            velocity.x += (velocity.y > 0) ? -360.0f : 360.0f;
        }
        if (Mathf.Abs (velocity.z) > thr)
        {
            velocity.x += (velocity.z > 0) ? -360.0f : 360.0f;
        }

        client.Send (address, velocity.x, velocity.y, velocity.z);

        prevDir = nextDir;
    }
}