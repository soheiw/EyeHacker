using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.XR;

public class HMDDirectionVelocitySender : MonoBehaviour
{
    uOscClient client;
    [SerializeField] string address;
    [SerializeField] bool sync = true;

    [SerializeField] Vector3 velocity;

    private Vector3 oldDir;
    private Vector3 newDir;

    // Use this for initialization
    void Start ()
    {
        client = FindObjectOfType<uOscClient> ();
        velocity = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!sync || client == null) return;

        Quaternion dir_q = InputTracking.GetLocalRotation(XRNode.Head);
        newDir = dir_q.eulerAngles;

        velocity = newDir - oldDir;
        if(Mathf.Abs(velocity.x) > 300.0f)
        {
            velocity.x = 360.0f - Mathf.Abs(velocity.x);
        }

        if(Mathf.Abs(velocity.y) > 300.0f)
        {
            velocity.y = 360.0f - Mathf.Abs(velocity.y);
        }

        if(Mathf.Abs(velocity.z) > 300.0f)
        {
            velocity.z = 360.0f - Mathf.Abs(velocity.z);
        }

        client.Send (address, velocity.x, velocity.y, velocity.z);

        oldDir = newDir;
    }
}