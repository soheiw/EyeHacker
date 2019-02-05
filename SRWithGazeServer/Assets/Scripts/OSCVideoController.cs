using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uOSC;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent (typeof (VideoPlayer))]
public class OSCVideoController : MonoBehaviour
{
    public string playerName = "Player1";
    public string imageName;
    public float startTime = 0.0f;

    [SerializeField] private uOscServer server;
    [SerializeField] private uOscClient client;
    public VideoPlayer videoPlayer;
    [SerializeField] private VideoPlayer maskPlayer;

    // Use this for initialization
    void Start ()
    {
        if (!server) server = FindObjectOfType<uOscServer> ();
        if (!client) client = FindObjectOfType<uOscClient> ();

        if (server) server.onDataReceived.AddListener (OnDataReceived);
        videoPlayer = GetComponent<VideoPlayer> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!videoPlayer) return;
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/server/" + playerName + "/startplaying")
        {
            if (message.values.Length > 1)
            {
                //PlayVideo (OSCVideo.directoryPath + (string) message.values[0], (float) message.values[1]);
                PlayVideo (OSCVideo.directoryPath + imageName, startTime);
            }
            else
            {
                // PlayVideo (OSCVideo.directoryPath + (string) message.values[0]);
                PlayVideo (OSCVideo.directoryPath + imageName);
            }
        }

        if (message.address == "/server/" + playerName + "/stopplaying")
        {
            StopVideo ();
        }

        if (message.address == "/server/" + playerName + "/setvideotime")
        {
            SetVideoTime ((float) message.values[0]);
        }

        if (message.address == "/server/" + playerName + "/getvideonames")
        {
            client.Send ((string) message.values[0], GetVideoNames ());
        }
    }

    public void PlayVideo (string path)
    {
        if (videoPlayer.isPlaying) videoPlayer.Stop ();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path + ".mp4";
        if (System.IO.File.Exists (videoPlayer.url)) videoPlayer.Play ();

        OSCVideo.players.Add (this.videoPlayer);

        if (maskPlayer != null)
        {
            maskPlayer.source = VideoSource.Url;
            maskPlayer.url = path + "_Mask.mp4";
            if (System.IO.File.Exists (maskPlayer.url)) maskPlayer.Play ();

            OSCVideo.players.Add (this.maskPlayer);
        }
    }

    public void PlayVideo (string path, float ratio)
    {
        PlayVideo (path);
        StartCoroutine (SetVideoNormalizedTime (ratio));
    }

    public void StopVideo ()
    {
        if (!videoPlayer.isPlaying) return;

        videoPlayer.Stop ();
        OSCVideo.players.Remove (this.videoPlayer);

        if (maskPlayer == null) return;
        if (!maskPlayer.isPlaying) return;

        maskPlayer.Stop ();
        OSCVideo.players.Remove (this.maskPlayer);
    }

    void SetVideoTime (float time_s)
    {
        if (!videoPlayer.isPlaying) return;

        // translate from sec to frame
        ulong frame = (ulong) (videoPlayer.frameRate * time_s);
        if (frame < videoPlayer.frameCount)
        {
            videoPlayer.frame = (long) frame;
            if (maskPlayer != null)
            {
                maskPlayer.frame = (long) frame;
            }
        }
        else
        {
            Debug.LogWarning ("Exceed time.");
        }
    }

    IEnumerator SetVideoNormalizedTime (float ratio)
    {
        yield return new WaitUntil (() => videoPlayer.isPlaying);
        float time_s = videoPlayer.frameCount / videoPlayer.frameRate * ratio;
        SetVideoTime (time_s);
    }

    string[] GetVideoNames ()
    {
        var patterns = new string[] { ".mp4" };
        var names = System.IO.Directory.GetFiles (OSCVideo.directoryPath).Where (file => patterns.Any (pattern => file.ToLower ().EndsWith (pattern))).ToArray<string> ();

        for (int i = 0; i < names.Length; i++)
        {
            names[i] = System.IO.Path.GetFileName (names[i]);
        }
        return names;
    }
}