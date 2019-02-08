using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwitchMasksDependingOnRisk : MonoBehaviour
{
    public InspectActiveHeatMap realtime;
    public InspectHeatMap[] past;
    public GameObject[] masks;

    public GameObject HMDImagePlayer;

    [Range (0, 2)]
    public float threshold;

    [Header ("Time")]
    public float realToPastTime = 2.0f;
    public float PastToRealTime = 2.0f;

    public float[] risks;
    private bool playPastImage;
    private float[] time;

    // Use this for initialization
    void Start ()
    {
        playPastImage = false;
        time = new float[past.Length];
        for (int j = 0; j < time.Length; j++)
        {
            time[j] = 0.0f;
        }
        risks = new float[past.Length];
        for (int i = 0; i < risks.Length; i++)
        {
            risks[i] = 0.0f;
        }

        HMDImagePlayer.GetComponent<Renderer> ().material.SetFloat ("_mask", 1.0f);
    }

    // Update is called once per frame
    void Update ()
    {
        bool gazeSomething = false;

        for (int i = 0; i < risks.Length; i++)
        {
            risks[i] = realtime.risk * realtime.risk + past[i].risk * past[i].risk;

            if (risks[i] < threshold && risks[i] >= 1.0f)
            {
                time[i] += Time.deltaTime;
            }
            else
            {
                // Debug.LogWarning ("change " + i + " is dangerous");
                time[i] = 0.0f;
            }
        }

        if (!playPastImage) // realtime再生中
        {
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] > realToPastTime)
                {
                    Debug.LogWarning ("change to past");
                    time[i] = 0.0f;
                    playPastImage = true;
                    StartCoroutine (MaskOn (i));
                }
            }
        }
        else
        {
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] > PastToRealTime)
                {
                    Debug.LogWarning ("change to realtime");
                    time[i] = 0.0f;
                    playPastImage = false;
                    StartCoroutine (MaskOff (i));
                }
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

    IEnumerator MaskOn (int num)
    {
        //DOTween.To (SetAlpha, 0.0f, 1.0f, 1.0f)
        //    .SetEase (Ease.InOutCubic);
        GameObject[] aroundMasks = past[num].aroundFixedMasks;
        for (int i = 0; i < aroundMasks.Length; i++)
        {
            aroundMasks[i].SetActive (true);
        }
        yield return null;
    }

    IEnumerator MaskOff (int num)
    {
        GameObject[] aroundMasks = past[num].aroundFixedMasks;
        for (int i = 0; i < aroundMasks.Length; i++)
        {
            aroundMasks[i].SetActive (false);
        }
        yield return null;
    }

}