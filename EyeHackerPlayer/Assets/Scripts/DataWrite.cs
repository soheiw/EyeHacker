using System;
using System.IO;
using UnityEngine;
using ViveSR.anipal.Eye;

public class DataWrite : MonoBehaviour
{
    DateTime datetimeStr;
    private string time;

    public OSCAngularVelocitySender angularVelocitySender;
    public SRanipal_GazeRaySample gazeRaySample;
    public OSCAlphaModifier alphaModifier;

    private float velocity;
    private Vector3 gazePosition;
    private Vector3 ballPosition;

    void Start ()
    {
        velocity = 0.0f;
        gazePosition = new Vector3 (0.0f, 0.0f, 0.0f);
        ballPosition = new Vector3 (0.0f, 0.0f, 0.0f);
    }
    void Update ()
    {
        datetimeStr = System.DateTime.Now;
        time = String.Format ("{0:yyyy/MM/dd HH:mm:ss:fff}", datetimeStr);
        velocity = angularVelocitySender.velocity.magnitude;
        gazePosition = gazeRaySample.gazePosition;
        if (alphaModifier.selectedBall != null)
        {
            ballPosition = alphaModifier.selectedBall.transform.position;
        }
        else
        {
            ballPosition = new Vector3 (0.0f, 0.0f, 0.0f);
        }
        LogSave ("logData");
    }

    public void LogSave (string fileName)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo (Application.dataPath + "/Resources/" + fileName + ".csv");
        sw = fi.AppendText ();
        sw.WriteLine (time + ", " + velocity.ToString () + ", " + gazePosition.x.ToString ("f4") + ", " + gazePosition.y.ToString ("f4") + ", " + gazePosition.z.ToString ("f4") + ", " + ballPosition.x.ToString ("f4") + ", " + ballPosition.y.ToString ("f4") + ", " + ballPosition.z.ToString ("f4"));
        sw.Flush ();
        sw.Close ();
    }
}