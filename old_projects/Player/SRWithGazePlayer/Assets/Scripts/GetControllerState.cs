using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetControllerState : MonoBehaviour
{
    [SerializeField] private GameObject leftController;
    // [SerializeField] private GameObject rightController;

    public bool leftControllerTouchpadPressUp;
    // public bool rightControllerTouchpadPressUp;

    public bool leftControllerTriggerPressUp;
    // public bool rightControllerTriggerPressUp;

    public bool leftControllerGripped;
    // public bool rightControllerGripped;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        var leftStatus = SteamVR_Controller.Input ((int) leftController.GetComponent<SteamVR_TrackedObject> ().index);
        // var rightStatus = SteamVR_Controller.Input ((int) rightController.GetComponent<SteamVR_TrackedObject> ().index);

        var touchpad = SteamVR_Controller.ButtonMask.Touchpad;
        var trigger = SteamVR_Controller.ButtonMask.Trigger;
        var grip = SteamVR_Controller.ButtonMask.Grip;

        leftControllerTouchpadPressUp = leftStatus.GetPressUp (touchpad);
        // rightControllerTouchpadPressUp = rightStatus.GetPressUp (touchpad);

        leftControllerTriggerPressUp = leftStatus.GetPressUp (trigger);
        // rightControllerTriggerPressUp = rightStatus.GetPressUp (trigger);

        leftControllerGripped = leftStatus.GetPressDown (grip);
        // rightControllerGripped = rightStatus.GetPressDown (grip);
    }
}