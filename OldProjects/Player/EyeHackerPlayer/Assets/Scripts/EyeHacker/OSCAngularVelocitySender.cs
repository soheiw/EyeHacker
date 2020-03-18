using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.XR;

public class OSCAngularVelocitySender : MonoBehaviour
{
    [SerializeField] private uOscClient client;
    [SerializeField] private string address;

    public Vector3 velocity;

    private Vector3 prevDir;
    private Vector3 nextDir;

    private Quaternion dir_p;
    private Quaternion dir_q;

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
        dir_p = InputTracking.GetLocalRotation (XRNode.Head);
        // prevDir = dir_p.eulerAngles;
    }

    // Update is called once per frame
    void Update ()
    {
        dir_q = InputTracking.GetLocalRotation (XRNode.Head);
        // nextDir = dir_q.eulerAngles;

        Quaternion change = dir_q * Quaternion.Inverse (dir_p);
        velocity = change.eulerAngles;
        velocity.x = Map360To180 (velocity.x);
        velocity.y = Map360To180 (velocity.y);
        velocity.z = Map360To180 (velocity.z);
        // velocity = nextDir - prevDir;

        // if (Mathf.Abs (velocity.x) > thr)
        // {
        //     velocity.x += (velocity.x > 0) ? -360.0f : 360.0f;
        // }
        // if (Mathf.Abs (velocity.y) > thr)
        // {
        //     velocity.x += (velocity.y > 0) ? -360.0f : 360.0f;
        // }
        // if (Mathf.Abs (velocity.z) > thr)
        // {
        //     velocity.x += (velocity.z > 0) ? -360.0f : 360.0f;
        // }

        client.Send (address, velocity.x, velocity.y, velocity.z);

        // prevDir = nextDir;
        dir_p = dir_q;
    }

    float Map360To180 (float degree)
    {
        if (degree < 180)
        {
            return degree;
        }

        return 360.0f - degree;
    }
}