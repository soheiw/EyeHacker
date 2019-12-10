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
    public GameObject sphere100;

    private float velocity;
    private Vector3 gazePosition;
    private Vector3 headPosition;
    private Vector3 relativePosition;
    private Vector3 ballPosition;
    private bool isExpNow;

    void Start ()
    {
        velocity = 0.0f;
        gazePosition = new Vector3 (0.0f, 0.0f, 0.0f);
        headPosition = new Vector3 (0.0f, 0.0f, 0.0f);
        relativePosition = new Vector3 (0.0f, 0.0f, 0.0f);
        ballPosition = new Vector3 (0.0f, 0.0f, 0.0f);
        isExpNow = false;
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

        RaycastHit headHit;
        if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out headHit))
        {
            headPosition = transform.TransformPoint (headHit.point);
        }
        else
        {
            headPosition = new Vector3 (0.0f, 0.0f, 0.0f);
        }

        isExpNow = !(sphere100.activeSelf);
        relativePosition = gazePosition - headPosition;

        LogSave ("logData");
    }

    public void LogSave (string fileName)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo (Application.dataPath + "/Resources/" + fileName + ".csv");
        sw = fi.AppendText ();
        sw.WriteLine (time + ", " +
            isExpNow + ", " +
            velocity.ToString () + ", " +
            gazePosition.x.ToString ("f4") + ", " + gazePosition.y.ToString ("f4") + ", " + gazePosition.z.ToString ("f4") + ", " + gazePosition.magnitude + ", " +
            headPosition.x.ToString ("f4") + ", " + headPosition.y.ToString ("f4") + ", " + headPosition.z.ToString ("f4") + ", " + headPosition.magnitude + ", " +
            relativePosition.x.ToString ("f4") + ", " + relativePosition.y.ToString ("f4") + ", " + relativePosition.z.ToString ("f4") + ", " + relativePosition.magnitude + ", " +
            ballPosition.x.ToString ("f4") + ", " + ballPosition.y.ToString ("f4") + ", " + ballPosition.z.ToString ("f4") + ", " + ballPosition.magnitude);
        sw.Flush ();
        sw.Close ();
    }
}