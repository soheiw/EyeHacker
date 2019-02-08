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
        client.Send (address, velocity.x, velocity.y, velocity.z);

        oldDir = newDir;
    }
}