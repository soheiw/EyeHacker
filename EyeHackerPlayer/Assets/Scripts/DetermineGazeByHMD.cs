using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DetermineGazeByHMD : MonoBehaviour
{
    // public Vector2 textureSize = new Vector2 (3008.0f, 1504.0f);
    private Vector3 gazePosition;
    public Vector2 gazeCoord;

    public int LengthOfRay = 25;
    [SerializeField] private LineRenderer GazeRayRenderer;

    // Start is called before the first frame update
    void Start ()
    {
        Assert.IsNotNull (GazeRayRenderer);
    }

    // Update is called once per frame
    void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            gazePosition = transform.TransformPoint (hit.point);
            gazeCoord = hit.textureCoord;
            // gazeCoord.x *= textureSize.x;
            // gazeCoord.y *= textureSize.y;
            // Debug.Log ("HMD gazeCoord: x: " + gazeCoord.x + ", y: " + gazeCoord.y);
        }
        else
        {
            gazePosition = transform.TransformPoint (Camera.main.transform.position + Camera.main.transform.forward * 50.0f);
            // gazeCoord = new Vector2 (textureSize.x * 0.5f, textureSize.y * 0.5f);
            gazeCoord = new Vector2 (0.5f, 0.5f);
            Debug.Log ("HMD raycast failed.");
        }

        GazeRayRenderer.SetPosition (0, Camera.main.transform.position - Camera.main.transform.up * 0.05f);
        GazeRayRenderer.SetPosition (1, Camera.main.transform.position + Camera.main.transform.forward * LengthOfRay);
    }
}