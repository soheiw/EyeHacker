using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateWholeRisk : MonoBehaviour
{
    public InspectActiveHeatMap realtime;
    public InspectActiveHeatMap past;

    public GameObject HMDImagePlayer;

    [Range (0, 1.414f)]
    public float threshold;

    private float wholeRisk;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        wholeRisk = realtime.risk * realtime.risk + past.risk * past.risk;
        Debug.Log((wholeRisk));
        if (wholeRisk < threshold * threshold)
        {
            HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", 1);
        }
        else
        {
            HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", 0);
        }
    }
}