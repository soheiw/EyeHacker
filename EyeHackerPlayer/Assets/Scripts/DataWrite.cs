using System;
using System.IO;
using UnityEngine;
using ViveSR.anipal.Eye;

public class DataWrite : MonoBehaviour
{
    public int[] logData; // Logデータの宣言
    DateTime datetimeStr;
    private string time;

    public OSCAngularVelocitySender angularVelocitySender;
    public SRanipal_GazeRaySample gazeRaySample;

    private float velocity;
    private Vector3 gazePosition;

    void Start ()
    {
        velocity = 0.0f;
        gazePosition = new Vector3 (0.0f, 0.0f, 0.0f);
    }
    void Update ()
    {
        datetimeStr = System.DateTime.Now;
        time = String.Format ("{0:yyyy/MM/dd HH:mm:ss:fff}", datetimeStr);
        velocity = angularVelocitySender.velocity.magnitude;
        gazePosition = gazeRaySample.gazePosition;
        LogSave (logData, "logData"); // Logデータをcsv形式で書き出す
    }

    public void LogSave (int[] x, string fileName)
    {
        StreamWriter sw; // これがキモらしい
        FileInfo fi;　　 // Aplication.dataPath で プロジェクトファイルがある絶対パスが取り込める
        fi = new FileInfo (Application.dataPath + "/Resources/" + fileName + ".csv");
        sw = fi.AppendText ();
        sw.WriteLine (time + ", " + velocity.ToString () + ", " + gazePosition.ToString ());
        sw.Flush ();
        sw.Close ();
    }
}