using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (FFmpegOut.ManualCameraCapture))]
public class HeatmapRecorderController : MonoBehaviour
{
    public FFmpegOut.ManualCameraCapture realtimeHeatmapCapture;
    public FFmpegOut.ManualCameraCapture pastHeatmapCapture;

    private bool isRecording = false;

    // Use this for initialization
    void Start ()
    {

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.H)){
            StartRecording("heatmap");
        }

        if(Input.GetKeyDown(KeyCode.J)){
            StopRecording();
        }
    }

    void OnDisable ()
    {
        if (isRecording)
        {
            StopRecording ();
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

        realtimeHeatmapCapture.StartCapturing ("Assets/StreamingAssets/Videos/" + "realtimeHeatmap");
        pastHeatmapCapture.StartCapturing ("Assets/StreamingAssets/Videos/" + "pastHeatmap");
        isRecording = true;
    }

    void StopRecording ()
    {
        realtimeHeatmapCapture.StopCapturing ();
        pastHeatmapCapture.StopCapturing();
        isRecording = false;
    }
}