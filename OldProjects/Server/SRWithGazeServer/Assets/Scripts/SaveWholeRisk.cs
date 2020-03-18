using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[RequireComponent (typeof (CalculateWholeRisk))]
[RequireComponent (typeof (AdjustThresholdByHMDRotation))]

public class SaveWholeRisk : MonoBehaviour
{
    private CalculateWholeRisk calculateWholeRisk;
    private AdjustThresholdByHMDRotation adjust;

    private int count;
    private bool isWriting;
    private StreamWriter sw;

    // Use this for initialization
    void Start ()
    {
        count = 0;
        isWriting = false;

        calculateWholeRisk = GetComponent<CalculateWholeRisk> ();
        adjust = GetComponent<AdjustThresholdByHMDRotation> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.O) && !isWriting)
        {
            sw = new StreamWriter ("Assets/StreamingAssets/Videos/wholeRisk" +
                System.DateTime.Now.Month.ToString () +
                System.DateTime.Now.Day.ToString () +
                System.DateTime.Now.Hour.ToString () +
                System.DateTime.Now.Minute.ToString () +
                System.DateTime.Now.Second.ToString () +
                ".csv", true);
            isWriting = true;
            Debug.Log ("start logging");
        }

        if (isWriting)
        {
            string[] str = { count.ToString (), Mathf.Sqrt (calculateWholeRisk.wholeRisk).ToString (), adjust.magnitude.ToString () };
            string strConnected = string.Join (",", str);
            sw.WriteLine (strConnected);
            count++;
        }

        if (Input.GetKeyDown (KeyCode.P) && isWriting)
        {
            sw.Close ();
            isWriting = false;
            Debug.Log ("end logging");
        }
    }
}