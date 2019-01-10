using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCPositionSyncSender : MonoBehaviour
{
    uOscClient client;
    [SerializeField] string address;
    [SerializeField] bool sync = true;
    [SerializeField] bool isLocal = false;

    // Use this for initialization
    void Start ()
    {
        client = FindObjectOfType<uOscClient> ();
    }

    public void Update ()
    {
        if (!client || !sync) return;

        var pos = isLocal ? transform.localPosition : transform.position;
        var rot = isLocal ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;

        client.Send (address, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z);
    }
}