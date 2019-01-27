﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectHeatMap : MonoBehaviour
{
    public OSCGazeCoordReceiver gazeCoordReceiver;
    public OSCMaskController maskController;
    public GameObject mask;
    public GameObject maskBlender;

    public bool isControlByOSC = false;

    private Renderer drawRenderer;
    private Texture2D bodyTexture;

    // Use this for initialization
    void Start ()
    {
        drawRenderer = this.GetComponent<Renderer> ();
        bodyTexture = (Texture2D) drawRenderer.material.mainTexture;
        var bodyPixels = bodyTexture.GetPixels ();
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
        if (!isControlByOSC)
        {
            mask.SetActive (CompareHeatmapValueToThreshold (degree));
            SetMaskSizeAlongWithHeatMapValue (degree);
            SetMaskAlphaAlongWithHeatMapValue (degree);
        }
    }

    float InspectHeatmapValueAtGazePoint (Vector2 pos)
    {
        Color col = bodyTexture.GetPixel ((int) pos.x, (int) pos.y);
        return (col.r + col.g + col.b) / 3.0f;
    }

    bool CompareHeatmapValueToThreshold (float val)
    {
        if (val > 0.2f)
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
}