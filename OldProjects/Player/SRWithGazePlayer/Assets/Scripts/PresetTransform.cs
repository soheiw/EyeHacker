using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetTransform : MonoBehaviour
{
    public Vector3 presetPosition;
    public Quaternion presetRotation;

    public GUIStyle textStyle;

    [SerializeField] private GetControllerState getControllerState;

    [SerializeField] private GameObject leftController;
    // [SerializeField] private GameObject rightController;

    // Use this for initialization
    void Start ()
    {
        // TODO: Findを使わない実装
        if (getControllerState == null)
        {
            getControllerState = GameObject.Find ("[CameraRig]").GetComponent<GetControllerState> ();
        }
        if (leftController == null)
        {
            leftController = GameObject.Find ("Controller (left)");
        }
        // if (rightController == null)
        // {
        //     rightController = GameObject.Find ("Controller (right)");
        // }
    }

    // Update is called once per frame
    void Update ()
    {
        if (getControllerState.leftControllerTriggerPressUp)
        {
            // FIXME: 結局controller自体を呼びに行くので冗長
            presetPosition = leftController.transform.position;
            presetRotation = leftController.transform.rotation;
        }
    }

    // void OnGUI ()
    // {
    //     GUI.Label (new Rect (50, 50, 50, 50), "px: " + presetPosition.x, textStyle);
    //     GUI.Label (new Rect (50, 100, 50, 50), "py: " + presetPosition.y, textStyle);
    //     GUI.Label (new Rect (50, 150, 50, 50), "pz: " + presetPosition.z, textStyle);
    //     Vector3 rot = presetRotation.eulerAngles;
    //     GUI.Label (new Rect (50, 200, 50, 50), "rx: " + rot.x, textStyle);
    //     GUI.Label (new Rect (50, 250, 50, 50), "ry: " + rot.y, textStyle);
    //     GUI.Label (new Rect (50, 300, 50, 50), "rz: " + rot.z, textStyle);
    // }
}