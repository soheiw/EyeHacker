using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    private Camera sceneCamera;
    private CalibrationDemo calibrationDemo;
    private LineRenderer heading;
    private List<MeshRenderer> gazeRenderers;
    private int intLayerRay;
    // private bool isRayOn;
    private GameObject mainCamera;

    private Vector3 standardViewportPoint = new Vector3 (0.5f, 0.5f, 10); // default marker position

    public Vector3 gazePosition;
    private Vector2 gazePointLeft;
    private Vector2 gazePointRight;
    private Vector2 gazePointCenter;

    [SerializeField] private GetControllerState getControllerState;

    public GameObject gazePos;
    public Vector2 gazeCoord;
    public Vector2 textureSize = new Vector2 (1280.0f, 720.0f);

    public float gazeDistanceThr = 100.0f;
    private bool isFirstEyeTrack;
    [SerializeField] private Vector3 prevGazePosition;

    private const float INF = 10000.0f;

    [SerializeField] private uOscServer server;

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
        // getControllerState = GameObject.Find ("[CameraRig]").GetComponent<GetControllerState> ();

        // TODO: tagで綺麗に取得
        mainCamera = GameObject.Find ("Main Camera");
        GameObject leftEye = mainCamera.transform.Find ("LeftEye_2D").gameObject;
        leftEye.layer = LayerMask.NameToLayer ("Ray");
        GameObject leftEyePoint = leftEye.transform.Find ("MarkerEye").gameObject;
        leftEyePoint.layer = LayerMask.NameToLayer ("Ray");
        GameObject rightEye = mainCamera.transform.Find ("RightEye_2D").gameObject;
        rightEye.layer = LayerMask.NameToLayer ("Ray");
        GameObject rightEyePoint = rightEye.transform.Find ("MarkerEye").gameObject;
        rightEyePoint.layer = LayerMask.NameToLayer ("Ray");
        GameObject gaze2D = mainCamera.transform.Find ("Gaze_2D").gameObject;
        gaze2D.layer = LayerMask.NameToLayer ("Ray");
        GameObject gaze2DPoint = gaze2D.transform.Find ("MarkerEye").gameObject;
        gaze2DPoint.layer = LayerMask.NameToLayer ("Ray");
        GameObject gaze3D = mainCamera.transform.Find ("Gaze_3D").gameObject;
        gaze3D.layer = LayerMask.NameToLayer ("Ray");
        GameObject gaze3DPoint = gaze3D.transform.Find ("MarkerEye").gameObject;
        gaze3DPoint.layer = LayerMask.NameToLayer ("Ray");

        gazeRenderers = new List<MeshRenderer> ();
        gazeRenderers.Add (leftEye.GetComponent<MeshRenderer> ());
        gazeRenderers.Add (leftEyePoint.GetComponent<MeshRenderer> ());
        gazeRenderers.Add (rightEye.GetComponent<MeshRenderer> ());
        gazeRenderers.Add (rightEyePoint.GetComponent<MeshRenderer> ());
        gazeRenderers.Add (gaze2D.GetComponent<MeshRenderer> ());
        gazeRenderers.Add (gaze2DPoint.GetComponent<MeshRenderer> ());
        gazeRenderers.Add (gaze3D.GetComponent<MeshRenderer> ());
        gazeRenderers.Add (gaze3DPoint.GetComponent<MeshRenderer> ());

        intLayerRay = LayerMask.NameToLayer ("Ray");
        // isRayOn = false;

        // server = FindObjectOfType<uOscServer> ();
        if (!server)
        {
            UnityEngine.Debug.Log ("OSCserver not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);

        /* if (calibrationDemo.enabled)
        {
            heading.enabled = false;
            for (int i = 0; i < gazeRenderers.Count; i++)
            {
                gazeRenderers[i].enabled = false;
            }
        } */

        isFirstEyeTrack = true;
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

        // bool isLeftGripped = getControllerState.leftControllerGripped;
        // bool isLeftGripped = getControllerState.leftControllerGripped;

        // if (Input.GetKeyUp (KeyCode.G))
        // if (Input.GetKeyUp (KeyCode.G) || isLeftGripped)
        // {
        //     calibrationDemo.enabled = !calibrationDemo.enabled;
        //     heading.enabled = calibrationDemo.enabled ? true : false;
        // }

        // if (Input.GetKeyUp (KeyCode.L))
        // if (Input.GetKeyUp (KeyCode.L) || isLeftGripped)
        // {
        //     if (calibrationDemo.enabled)
        //     {
        //         /* heading.enabled = !heading.enabled;
        //         for (int i = 0; i < gazeRenderers.Count; i++)
        //         {
        //             bool isRendered = gazeRenderers[i].enabled;
        //             gazeRenderers[i].enabled = !isRendered;
        //         } */

        //         if (isRayOn)
        //         {
        //             mainCamera.GetComponent<Camera> ().cullingMask &= ~(1 << intLayerRay);
        //         }
        //         else
        //         {
        //             mainCamera.GetComponent<Camera> ().cullingMask |= (1 << intLayerRay);
        //         }
        //         isRayOn = !isRayOn;
        //     }
        // }

        Ray ray = sceneCamera.ViewportPointToRay (viewportPoint);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit))
        {
            gazePosition = hit.point;
            gazeCoord = hit.textureCoord;
            gazeCoord.x *= textureSize.x;
            gazeCoord.y *= textureSize.y;
        }
        else
        {
            gazePosition = ray.origin + ray.direction * 50f;
            gazeCoord = new Vector2 (INF, INF);
        }

        gazePos.transform.position = gazePosition;
        gazePos.transform.rotation = Quaternion.Euler (hit.normal);

        if (heading.enabled)
        {
            heading.SetPosition (0, sceneCamera.transform.position - sceneCamera.transform.up); // 下方向にちょっとずらす
            heading.SetPosition (1, gazePosition);
        }
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/viewray")
        {
            var state = message.values[0].ToString ();
            if (state == "1")
            {
                mainCamera.GetComponent<Camera> ().cullingMask &= ~(1 << intLayerRay);
            }
            else
            {
                mainCamera.GetComponent<Camera> ().cullingMask |= (1 << intLayerRay);
            }
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