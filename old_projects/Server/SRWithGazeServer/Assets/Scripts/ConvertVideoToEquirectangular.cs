using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertVideoToEquirectangular : MonoBehaviour
{
    private Camera renderingCamera;
    [SerializeField]
    RenderTexture equirectangulerTexture;

    [SerializeField]
    RenderTexture cubemapTexture;
    // Use this for initialization
    void Start ()
    {
        renderingCamera = GetComponent<Camera> ();
    }

    // Update is called once per frame
    void Update ()
    {
        renderingCamera.RenderToCubemap (cubemapTexture);
        cubemapTexture.ConvertToEquirect (equirectangulerTexture);
    }
}