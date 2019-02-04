using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectActiveHeatMap : MonoBehaviour
{
    public RenderTexture comparedImage;
    public int perPixel = 8;

    private Texture2D texture2D;

    public OSCGazeCoordReceiver gazeCoordReceiver;

    public GameObject mask;

    public float risk;

    // Use this for initialization
    void Start ()
    {
        texture2D = new Texture2D (comparedImage.width, comparedImage.height, TextureFormat.ARGB32, false);
        texture2D.filterMode = FilterMode.Point;
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

        StartCoroutine (ReadImage ());

        Color pixel = texture2D.GetPixel ((int) hitPoint.x / perPixel, (int) hitPoint.y / perPixel);
        risk = (pixel.r + pixel.g + pixel.b) / 3.0f;
    }

    IEnumerator ReadImage ()
    {
        yield return new WaitForEndOfFrame ();
        RenderTexture.active = comparedImage;
        texture2D.ReadPixels (new Rect (0, 0, 1280, 720), 0, 0);
        texture2D.Apply ();
    }
}