using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCGazePixelSender : MonoBehaviour
{
    public InspectActiveHeatMap inspectActiveHeatmap;

    uOscClient client;
    [SerializeField] string address;
    [SerializeField] bool sync = true;

    // Use this for initialization
    void Start ()
    {
        client = FindObjectOfType<uOscClient> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!sync || client == null) return;
        client.Send (address, inspectActiveHeatmap.centerPixel.x, inspectActiveHeatmap.centerPixel.y);
    }
}