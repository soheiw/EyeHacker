using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    private Camera sceneCamera;
    private CalibrationDemo calibrationDemo;
    private LineRenderer heading;

    private Vector3 standardViewportPoint = new Vector3 (0.5f, 0.5f, 10); // default marker position

    public Vector3 gazePosition;
    private Vector2 gazePointLeft;
    private Vector2 gazePointRight;
    private Vector2 gazePointCenter;

    [SerializeField] private GetControllerState getControllerState;

    public GameObject gazePos;

    // public Material shaderMaterial;

    // public bool monoColorMode = true;

    // Use this for initialization
    void Start ()
    {
        PupilData.calculateMovingAverage = false;

        sceneCamera = gameObject.GetComponent<Camera> ();
        calibrationDemo = gameObject.GetComponent<CalibrationDemo> ();
        heading = gameObject.GetComponent<LineRenderer> ();

        // TODO: Findを使わない実装
        getControllerState = GameObject.Find ("[CameraRig]").GetComponent<GetControllerState> ();
    }

    void OnEnable ()
    {
        if (PupilTools.IsConnected)
        {
            PupilTools.IsGazing = true;
            PupilTools.SubscribeTo ("gaze");
        }
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 viewportPoint = standardViewportPoint;

        if (PupilTools.IsConnected && PupilTools.IsGazing)
        {
            gazePointLeft = PupilData._2D.GetEyePosition (sceneCamera, PupilData.leftEyeID);
            gazePointRight = PupilData._2D.GetEyePosition (sceneCamera, PupilData.rightEyeID);
            gazePointCenter = PupilData._2D.GazePosition;
            viewportPoint = new Vector3 (gazePointCenter.x, gazePointCenter.y, 1f);
        }

        // if (Input.GetKeyUp(KeyCode.M))
        //     monoColorMode = !monoColorMode;

        bool isLeftGripped = getControllerState.leftControllerGripped;
        bool isRightGripped = getControllerState.rightControllerGripped;

        // if (Input.GetKeyUp (KeyCode.G))
        if (Input.GetKeyUp (KeyCode.G) || isLeftGripped)
        {
            calibrationDemo.enabled = !calibrationDemo.enabled;
            heading.enabled = calibrationDemo.enabled ? true : false;
        }

        // if (Input.GetKeyUp (KeyCode.L))
        if (Input.GetKeyUp (KeyCode.L) || isRightGripped)
            if (calibrationDemo.enabled)
                heading.enabled = !heading.enabled;

        if (heading.enabled)
        {
            heading.SetPosition (0, sceneCamera.transform.position - sceneCamera.transform.up); // 下方向にちょっとずらす

            Ray ray = sceneCamera.ViewportPointToRay (viewportPoint);
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit))
            {
                gazePosition = hit.point;
            }
            else
            {
                gazePosition = ray.origin + ray.direction * 50f;
            }
            heading.SetPosition (1, gazePosition);

            // gazePos.transform.position = gazePosition;
            // gazePos.transform.rotation = Quaternion.Euler (hit.normal);
    }
}

/*void OnRenderImage(RenderTexture source, RenderTexture destination)
{
    if (monoColorMode)
    {
        shaderMaterial.SetFloat("_highlightThreshold", 0.05f);
        switch (sceneCamera.stereoActiveEye)
        {
            case Camera.MonoOrStereoscopicEye.Left:
                shaderMaterial.SetVector("_viewportGazePosition", gazePointLeft);
                break;
            case Camera.MonoOrStereoscopicEye.Right:
                shaderMaterial.SetVector("_viewportGazePosition", gazePointRight);
                break;
            default:
                shaderMaterial.SetVector("_viewportGazePosition", gazePointCenter);
                break;
        }
        Graphics.Blit(source, destination, shaderMaterial);
    }
    else
        Graphics.Blit(source, destination);

}*/

}