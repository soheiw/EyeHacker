using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectHeatMap : MonoBehaviour
{
    public OSCGazeCoordReceiver gazeCoordReceiver;

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
        if (hitPoint.x < 0 || hitPoint.x > 1280 || hitPoint.y <= 0 || hitPoint.y > 720)
        {
            return;
        }

        Color col = bodyTexture.GetPixel ((int) hitPoint.x, (int) hitPoint.y);
        Debug.Log (col);
        if (col.r == 1 && col.g == 1 && col.b == 1)
        {
            Debug.Log ("nyan");
        }
    }
}