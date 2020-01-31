using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraController : MonoBehaviour
{
    public Vector3 fixedPos = new Vector3 (0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 trackingPos = InputTracking.GetLocalPosition (XRNode.CenterEye);

        var scale = transform.localScale;
        trackingPos = new Vector3 (trackingPos.x * scale.x, trackingPos.y * scale.y, trackingPos.z * scale.z);

        trackingPos = transform.rotation * trackingPos;
        transform.position = fixedPos - trackingPos;
    }
}