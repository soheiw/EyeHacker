using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectHeatMap : MonoBehaviour
{
    public OSCGazeCoordReceiver gazeCoordReceiver;
    public OSCMaskController maskController;
    public GameObject mask;
    public GameObject maskBlender;

    [Header ("Mask")]
    public bool isControlByOSC = false;
    public bool setMaskFixed;
    public bool switchMaskHere;
    public GameObject[] hereFixedMasks;
    public GameObject[] aroundFixedMasks;

    private Renderer drawRenderer;
    private Texture2D bodyTexture;

    // Use this for initialization
    void Start ()
    {
        drawRenderer = this.GetComponent<Renderer> ();
        bodyTexture = (Texture2D) drawRenderer.material.mainTexture;
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
        if (setMaskFixed)
        {
            // 固定のmaskがrayの位置に発生
            if (switchMaskHere)
            {
                for (int i = 0; i < hereFixedMasks.Length; i++)
                {
                    hereFixedMasks[i].SetActive (CompareHeatmapValueToThreshold (degree));
                }
            }
            // 固定のmaskがrayの位置でないところに発生
            else
            {
                for (int i = 0; i < aroundFixedMasks.Length; i++)
                {
                    if (!CompareHeatmapValueToThreshold (degree)) return;
                    aroundFixedMasks[i].SetActive (true);
                }
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

        if (time_sec > 3.0f)
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