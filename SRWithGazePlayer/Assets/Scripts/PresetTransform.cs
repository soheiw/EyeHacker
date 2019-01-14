using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetTransform : MonoBehaviour
{
    public Vector3 presetPosition;
    public Quaternion presetRotation;

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

        // if (getControllerState.rightControllerTriggerPressUp)
        // {
        //     // FIXME: 結局controller自体を呼びに行くので冗長
        //     presetTransform.position = rightController.transform.position;
        //     presetTransform.rotation = rightController.transform.rotation;
        // }
    }
}