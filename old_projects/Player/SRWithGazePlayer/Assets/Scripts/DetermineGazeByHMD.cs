using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetermineGazeByHMD : MonoBehaviour
{
    public GameObject gazePos;

    public Vector3 gazePosition;
    public Vector2 gazeCoord;
    public Vector2 textureSize = new Vector2 (1280.0f, 720.0f);

    private Camera sceneCamera;

    private const float INF = 10000.0f;

    private LineRenderer heading;

    // Use this for initialization
    void Start ()
    {
        sceneCamera = gameObject.GetComponent<Camera> ();
        heading = gameObject.GetComponent<LineRenderer> ();
    }

    // Update is called once per frame
    void Update ()
    {
        Ray ray = new Ray (sceneCamera.transform.position, sceneCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit))
        {
            gazePosition = hit.point;
            gazeCoord = hit.textureCoord;
            // gazeCoord.x *= textureSize.x;
            // gazeCoord.y *= textureSize.y;
        }
        else
        {
            gazePosition = ray.origin + ray.direction * 50f;
            gazeCoord = new Vector2 (0.5f, 0.5f);
        }
        gazePos.transform.position = gazePosition;
        gazePos.transform.rotation = Quaternion.Euler (hit.normal);

        if (heading.enabled)
        {
            heading.SetPosition (0, sceneCamera.transform.position - sceneCamera.transform.up); // 下方向にちょっとずらす
            heading.SetPosition (1, gazePosition);
        }
    }
}