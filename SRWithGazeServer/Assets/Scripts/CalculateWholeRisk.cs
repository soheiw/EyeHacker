using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CalculateWholeRisk : MonoBehaviour
{
    public InspectActiveHeatMap realtime;
    public InspectActiveHeatMap past;

    public GameObject HMDImagePlayer;

    [Range (0, 100)]
    public float originalThreshold;
    [SerializeField] float threshold;

    [Header ("Time")]
    public float realToPastTime = 2.0f;
    public float PastToRealTime = 2.0f;

    private float wholeRisk;
    private bool playPastImage;
    private float time;

    public GUIStyle textStyle;

    public bool adjustThreshold;
    private AdjustThresholdByHMDRotation adjust;

    // Use this for initialization
    void Start ()
    {
        playPastImage = false;
        time = 0.0f;
        adjust = GetComponent<AdjustThresholdByHMDRotation> ();
    }

    // Update is called once per frame
    void Update ()
    {
        wholeRisk = realtime.risk * realtime.risk + past.risk * past.risk;
        // Debug.Log ((wholeRisk));
        if (adjustThreshold)
        {
            if(adjust.magnitude > 1.0f)
            {
                threshold = originalThreshold * adjust.magnitude;
            }
            else
            {
                threshold = originalThreshold;
            }
        }
        else
        {
            threshold = originalThreshold;
        }

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
                StartCoroutine (MaskOn ());
            }
        }
        else
        {
            if (time > PastToRealTime)
            {
                Debug.LogWarning ("change to realtime");
                time = 0.0f;
                playPastImage = false;
                StartCoroutine (MaskOff ());
            }
        }

        if (playPastImage)
        {
            // HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", 1);
        }
        else
        {
            // HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", 0);
        }
    }

    IEnumerator MaskOn ()
    {
        DOTween.To (SetAlpha, 0.0f, 1.0f, 1.0f)
            .SetEase (Ease.InOutCubic);
        yield return null;
    }

    IEnumerator MaskOff ()
    {
        DOTween.To (SetAlpha, 1.0f, 0.0f, 1.0f)
            .SetEase (Ease.InOutCubic);
        yield return null;
    }

    void SetAlpha (float val)
    {
        HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", val);
    }

    void OnGUI ()
    {
        GUI.Label (new Rect (50, 300, 50, 50), "Whole Risk: " + wholeRisk, textStyle);
    }
}