using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationDemo : MonoBehaviour
{
    private GameObject pupilManager;
    void OnEnable ()
    {
        pupilManager = GameObject.Find ("Pupil Manager");
        if (PupilTools.IsConnected)
        {
            PupilGazeTracker.Instance.StartVisualizingGaze ();
            print ("We are gazing");
        }
    }

    void OnDisable ()
    {
        if (PupilTools.IsConnected)
        {
            PupilGazeTracker.Instance.StopVisualizingGaze ();
            print ("Stop gaze measurement");
            pupilManager.GetComponent<FramePublishing> ().enabled = true;
            pupilManager.transform.Find ("Camera/CalibrateText").gameObject.GetComponent<Text> ().text = "Wait a moment until\ngaze point calibration starts.";
        }
    }
}