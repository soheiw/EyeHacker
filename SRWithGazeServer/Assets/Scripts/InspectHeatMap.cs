using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectHeatMap : MonoBehaviour
{
    public OSCGazeCoordReceiver gazeCoordReceiver;
    public OSCMaskController maskController;
    public GameObject mask;

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
        
        if (degree == 1.0f)
        {
            mask.SetActive (true);
        }
        else
        {
            mask.SetActive (false);
        }

        mask.transform.localScale = maskController.originalScale * degree;
    }

    float InspectHeatmapValueAtGazePoint (Vector2 pos)
    {
        Color col = bodyTexture.GetPixel ((int) pos.x, (int) pos.y);
        return (col.r + col.g + col.b) / 3.0f;
    }
}