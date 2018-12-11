using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.XR;

public class HMDDirectionSender : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InputTracking.Recenter();
        OSCHandler.Instance.Init();
        OSCHandler.Instance.CreateClient("Unity", IPAddress.Parse("127.0.0.1"), 9000);
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion dir_q = InputTracking.GetLocalRotation(XRNode.Head);
        Vector3 dir = dir_q.eulerAngles;
        Debug.Log("HMD rotation: " + dir);
        var HMDdir = new List<float>() { dir.x, dir.y, dir.z };
        OSCHandler.Instance.SendMessageToClient("Unity", "/HMDdir", HMDdir);
        /*OSCHandler.Instance.SendMessageToClient("Unity", "/HMDdir/x", dir.x);
        OSCHandler.Instance.SendMessageToClient("Unity", "/HMDdir/y", dir.y);
        OSCHandler.Instance.SendMessageToClient("Unity", "/HMDdir/z", dir.z);*/
    }
}
