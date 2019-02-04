using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateWholeRisk : MonoBehaviour
{
    public InspectActiveHeatMap realtime;
    public InspectActiveHeatMap past;

    public GameObject HMDImagePlayer;

    [Range (0, 2)]
    public float threshold;

    [Header ("Time")]
    public float realToPastTime = 2.0f;
    public float PastToRealTime = 2.0f;

    private float wholeRisk;
    private bool playPastImage;
    private float time;

    // Use this for initialization
    void Start ()
    {
        playPastImage = false;
        time = 0.0f;
    }

    // Update is called once per frame
    void Update ()
    {
        wholeRisk = realtime.risk * realtime.risk + past.risk * past.risk;
        Debug.Log ((wholeRisk));

        if (wholeRisk < threshold)
        {
            time += Time.deltaTime;
        }
        else
        {
            Debug.LogWarning ("change is dangerous");
            time = 0.0f;
        }

        if (!playPastImage) // realtime再生中
        {
            if (time > realToPastTime)
            {
                Debug.LogWarning ("change to past");
                time = 0.0f;
                playPastImage = true;
            }
        }
        else
        {
            if (time > PastToRealTime)
            {
                Debug.LogWarning ("change to realtime");
                time = 0.0f;
                playPastImage = false;
            }
        }

        if (playPastImage)
        {
            HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", 1);
        }
        else
        {
            HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", 0);
        }
    }
}