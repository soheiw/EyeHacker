using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectActiveHeatMap : MonoBehaviour
{
    public RenderTexture comparedImage;
    public int perPixel = 8;

    public Texture2D texture2D;

    public OSCGazeCoordReceiver gazeCoordReceiver;

    public GameObject mask;

    public float risk;

    public bool fixPastHeatmap = false;

    public int edgeSize = 0;

    // Use this for initialization
    void Start ()
    {
        if (!fixPastHeatmap)
        {
            texture2D = new Texture2D (comparedImage.width, comparedImage.height, TextureFormat.ARGB32, false);
            texture2D.filterMode = FilterMode.Point;
        }
        risk = 1.0f;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 hitPoint = gazeCoordReceiver.gazePoint;
        if (hitPoint.x < 0 || hitPoint.x > 1280 || hitPoint.y < 0 || hitPoint.y > 720)
        {
            return;
        }

        risk = 0.0f;

        if (!fixPastHeatmap)
        {
            StartCoroutine (ReadImage ());
            Vector2Int centerPixel = new Vector2Int (159 - (int) hitPoint.x / perPixel, 89 - (int) hitPoint.y / perPixel);
            for (int x = centerPixel.x - edgeSize; x <= centerPixel.x + edgeSize; x++)
            {
                for (int y = centerPixel.y - edgeSize; y <= centerPixel.y + edgeSize; y++)
                {
                    if (x >= 0 && x <= 159 && y >= 0 && y <= 89)
                    {
                        Color pixel = texture2D.GetPixel (x, y);
                        risk += (pixel.r + pixel.g + pixel.b) / 3.0f;
                    }
                    else
                    {
                        // risk += 0.0f;
                    }
                }
            }
        }
        else
        {
            Color pixel = texture2D.GetPixel ((int) hitPoint.x, (int) hitPoint.y);
            Debug.Log (hitPoint);
            risk = (pixel.r + pixel.g + pixel.b) / 3.0f;
        }
    }

    IEnumerator ReadImage ()
    {
        yield return new WaitForEndOfFrame ();
        if (!fixPastHeatmap)
        {
            RenderTexture.active = comparedImage;
            texture2D.ReadPixels (new Rect (0, 0, 160, 90), 0, 0);
            texture2D.Apply ();
        }
    }
}