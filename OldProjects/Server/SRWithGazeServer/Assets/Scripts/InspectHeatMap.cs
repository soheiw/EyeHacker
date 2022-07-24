﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InspectHeatMap : MonoBehaviour
{
    public OSCGazeCoordReceiver gazeCoordReceiver;
    public OSCMaskController maskController;
    public GameObject mask;
    public GameObject maskBlender;

    public bool withPlayingVideo;
    [SerializeField] OSCVideoController videoController;
    private bool alreadyPlayed;

    [Header ("Mask")]
    public bool isControlByOSC = false;
    public bool setMaskFixed;
    public bool switchMaskHere;
    public GameObject[] hereFixedMasks;
    public GameObject[] aroundFixedMasks;
    public float timeThreshold = 3.0f;

    private Renderer drawRenderer;
    private Texture2D bodyTexture;

    public float risk;

    // Use this for initialization
    void Start ()
    {
        drawRenderer = this.GetComponent<Renderer> ();
        bodyTexture = (Texture2D) drawRenderer.material.mainTexture;
        if (withPlayingVideo)
        {
            alreadyPlayed = false;
            videoController.PlayVideo (OSCVideo.directoryPath + videoController.imageName, videoController.startTime);

            // TODO: 頭出しをWaitToPlay内で秒数を決め打ちしてやっているが，それをVideoPlayerのメソッドを駆使して書き換える
            StartCoroutine (WaitToPlay ());
            risk = 1.0f;
        }
    }

    private void OnEnable ()
    {
        if (withPlayingVideo)
        {
            alreadyPlayed = false;
            videoController.PlayVideo (OSCVideo.directoryPath + videoController.imageName, videoController.startTime);
            StartCoroutine (WaitToPlay ());
        }
    }

    private void OnDisable ()
    {
        if (withPlayingVideo)
        {
            videoController.StopVideo ();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 hitPoint = gazeCoordReceiver.gazePoint;
        if (hitPoint.x < 0 || hitPoint.x > 1280 || hitPoint.y < 0 || hitPoint.y > 720)
        {
            return;
        }

        float degree = InspectHeatmapValueAtGazePoint (hitPoint);
        risk = degree;
        if (setMaskFixed)
        {
            // 固定のmaskがrayの位置に発生
            if (switchMaskHere)
            {
                if (withPlayingVideo)
                {
                    for (int i = 0; i < hereFixedMasks.Length; i++)
                    {
                        // hereFixedMasks[i].SetActive (CompareHeatmapValueToThreshold (degree));
                        if (!CompareHeatmapValueToThreshold (degree)) return;
                        hereFixedMasks[i].SetActive (true);
                    }
                    if (!alreadyPlayed)
                    {
                        alreadyPlayed = true;
                        videoController.videoPlayer.Play ();
                    }
                }
                else
                {
                    for (int i = 0; i < hereFixedMasks.Length; i++)
                    {
                        // hereFixedMasks[i].SetActive (CompareHeatmapValueToThreshold (degree));
                        if (!CompareHeatmapValueToThreshold (degree)) return;
                        hereFixedMasks[i].SetActive (true);
                    }
                }
            }
            // 固定のmaskがrayの位置でないところに発生
            else
            {
                /* for (int i = 0; i < aroundFixedMasks.Length; i++)
                {
                    if (!CompareHeatmapValueToThreshold (degree)) return;
                    aroundFixedMasks[i].SetActive (true);
                } */
            }
        }
        else
        // 視線の先にmaskがついて回る
        {
            if (CompareHeatmapValueToThreshold (degree))
            {
                mask.SetActive (true);
                // if (isControlByOSC) return;
                // SetMaskSizeAlongWithHeatMapValue (degree);
                // SetMaskAlphaAlongWithHeatMapValue (degree);
            }
            else
            {
                mask.SetActive (false);
                // mask.transform.localScale = maskController.originalScale;
                // maskBlender.GetComponent<OSCMaskController> ().realtime.SetFloat ("_AdjustAlpha", 1.0f);
            }
        }
    }

    float InspectHeatmapValueAtGazePoint (Vector2 pos)
    {
        Color col = bodyTexture.GetPixel ((int) pos.x, (int) pos.y);
        return (col.r + col.g + col.b) / 3.0f;
    }

    float time_sec = 0.0f;
    bool CompareHeatmapValueToThreshold (float val)
    {
        if (val > 0.2f)
        {
            time_sec += Time.deltaTime;
        }
        else
        {
            time_sec = 0.0f;
        }

        if (time_sec > timeThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetMaskSizeAlongWithHeatMapValue (float val)
    {
        mask.transform.localScale = maskController.originalScale * val;
    }

    void SetMaskAlphaAlongWithHeatMapValue (float val)
    {
        maskBlender.GetComponent<OSCMaskController> ().realtime.SetFloat ("_AdjustAlpha", val);
    }

    IEnumerator WaitToPlay ()
    {
        yield return new WaitForSeconds (0.6f);
        videoController.videoPlayer.Pause ();
    }
}