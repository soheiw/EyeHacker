using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTransformByController : MonoBehaviour
{
    public Transform leftTarget;
    public Transform rightTarget;

    [SerializeField] private GetControllerState getControllerState;

    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    // Use this for initialization
    void Start ()
    {
        // TODO: Findを使わない実装
        getControllerState = GameObject.Find ("[CameraRig]").GetComponent<GetControllerState> ();
        leftController = GameObject.Find ("Controller (left)");
        rightController = GameObject.Find ("Controller (right)");

        PresetTransform presetTransform = GameObject.Find ("PresetTransform").GetComponent<PresetTransform> ();
        if (presetTransform != null)
        {
            leftTarget.transform.position = presetTransform.presetPosition;
            leftTarget.transform.rotation = presetTransform.presetRotation;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (getControllerState.leftControllerTriggerPressUp)
        {
            if (!leftTarget) return;
            // FIXME: 結局controller自体を呼びに行くので冗長
            leftTarget.transform.position = leftController.transform.position;
            leftTarget.transform.rotation = leftController.transform.rotation;
        }

        if (getControllerState.rightControllerTriggerPressUp)
        {
            if (!rightTarget) return;
            // FIXME: 結局controller自体を呼びに行くので冗長
            rightTarget.transform.position = rightController.transform.position;
            rightTarget.transform.rotation = rightController.transform.rotation;
        }
    }
}