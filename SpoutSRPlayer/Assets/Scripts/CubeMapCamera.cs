using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapCamera : MonoBehaviour {
    public Camera camera;
    public Cubemap cubemap;
    public Material conversionMaterial;

    [Header("Size")]
    public int outputWidth = 4096;
    public int outputHeight = 2048;
    public int cubeWidth = 1280;

    [Header("Texture")]
    public RenderTexture renderTexture;
    private Texture2D equirectangularTexture;

    // Use this for initialization
    void Start () {
        if (!camera)
        {
            camera = Camera.main;
        }
        if (!cubemap)
        {
            cubemap = new Cubemap(cubeWidth, TextureFormat.RGBA32, false);
        }
        if (!conversionMaterial)
        {
            Shader conversionShader = Shader.Find("Conversion/CubemapToEquirectangular");
            conversionMaterial = new Material(conversionShader);
        }

        equirectangularTexture = new Texture2D(outputWidth, outputHeight, TextureFormat.ARGB32, false);
	}
	
	// Update is called once per frame
	void Update () {
        camera.RenderToCubemap(cubemap);
        Graphics.Blit(cubemap, renderTexture, conversionMaterial);
        equirectangularTexture.ReadPixels(new Rect(0, 0, outputWidth, outputHeight), 0, 0, false);
        equirectangularTexture.Apply();
	}
}
