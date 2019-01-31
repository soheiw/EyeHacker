using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapLevelCalculator : MonoBehaviour
{
    public int overlapCount;
    public RenderTexture comparedImage;

    private Texture2D texture2D;
    private int count;

    // Use this for initialization
    void Start ()
    {
        overlapCount = 0;
        texture2D = new Texture2D (comparedImage.width, comparedImage.height, TextureFormat.ARGB32, false);
        texture2D.filterMode = FilterMode.Point;
    }

    // Update is called once per frame
    void Update ()
    {
        count = 0;
        StartCoroutine (ReadImage ());

        Color[] pixels = texture2D.GetPixels ();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r == 1.0 && pixels[i].g == 0.0)
            {
                count++;
            }
        }
        overlapCount = count;
    }

    IEnumerator ReadImage ()
    {
        yield return new WaitForEndOfFrame ();
        RenderTexture.active = comparedImage;
        texture2D.ReadPixels (new Rect (0, 0, 1280, 720), 0, 0);
        texture2D.Apply ();
    }
}