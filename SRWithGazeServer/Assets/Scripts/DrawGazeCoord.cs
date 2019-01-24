using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGazeCoord : MonoBehaviour
{
    public OSCGazeCoordReceiver gazeCoordReceiver;

    private Renderer drawRenderer;
    private Texture2D drawTexture;
    private Color[] textureBuffer;

    private int brushSize = 10;
    // Use this for initialization
    void Start ()
    {

        drawRenderer = this.GetComponent<Renderer> ();
        var bodyTexture = (Texture2D) drawRenderer.material.mainTexture;
        var bodyPixels = bodyTexture.GetPixels ();
        this.textureBuffer = new Color[bodyPixels.Length];
        bodyPixels.CopyTo (this.textureBuffer, 0);

        this.drawTexture = new Texture2D (bodyTexture.width, bodyTexture.height, TextureFormat.RGBA32, false);
        this.drawTexture.filterMode = FilterMode.Point;
        this.drawTexture.SetPixels (this.textureBuffer);
        this.drawTexture.Apply ();
        drawRenderer.material.mainTexture = this.drawTexture;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 hitPoint = gazeCoordReceiver.gazePoint;

        // brushSize分のピクセルを塗りつぶす
        for (int x = (int) (hitPoint.x - brushSize / 2); x < hitPoint.x + brushSize; x++)
        {
            for (int y = (int) (hitPoint.y - brushSize / 2); y < hitPoint.y + brushSize; y++)
            {
                if (x >= 0 && y >= 0)
                {
                    this.textureBuffer.SetValue (Color.white, (int) x + drawTexture.width * (int) y);
                }
            }
        }

        // テクスチャを適用
        this.drawTexture.SetPixels (this.textureBuffer);
        this.drawTexture.Apply ();
        drawRenderer.material.mainTexture = this.drawTexture;
    }
}