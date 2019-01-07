﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationDemo : MonoBehaviour
{
    void OnEnable ()
    {
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
        }
    }
}