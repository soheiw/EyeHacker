using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

[RequireComponent (typeof (FFmpegOut.ManualCameraCapture))]
public class OSCVideoRecorder : MonoBehaviour
{

    public string recorderName = "Recorder1";
    [SerializeField] private uOscServer server;

    private FFmpegOut.ManualCameraCapture cameraCapture;
    [SerializeField] private FFmpegOut.ManualCameraCapture maskCapture;

    private bool isRecording = false;

    void Awake ()
    {
        if (!server)
        {
            server = FindObjectOfType<uOscServer> ();
        }
        cameraCapture = GetComponent<FFmpegOut.ManualCameraCapture> ();
    }
    // Use this for initialization
    void Start ()
    {
        if (!server) return;
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDisable ()
    {
        if (isRecording)
        {
            StopRecording ();
        }
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/server/" + recorderName + "/startrecording")
        {
            if (!isRecording)
            {
                StartRecording ((string) message.values[0]);
            }
        }
        if (message.address == "/server/" + recorderName + "/stoprecording")
        {
            if (isRecording)
            {
                StopRecording ();
            }
        }
    }

    void StartRecording (string fileName)
    {
        // アクセス権確保のためすべてのプレーヤーを停止
        foreach (var player in OSCVideo.players)
        {
            player.Stop ();
        }
        OSCVideo.players.Clear ();

        cameraCapture.StartCapturing ("Assets/StreamingAssets/Videos/" + fileName);
        if (maskCapture != null) maskCapture.StartCapturing ("Assets/StreamingAssets/Videos/" + fileName + "_Mask");
        isRecording = true;
    }

    void StopRecording ()
    {
        cameraCapture.StopCapturing ();
        if (maskCapture != null) maskCapture.StopCapturing ();
        isRecording = false;
    }
}