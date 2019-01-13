using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OSCVideo
{
    public static string directoryPath
    {
        get
        {
            return System.IO.Path.Combine (Application.streamingAssetsPath, "Videos\\");
        }
    }

    public static List<VideoPlayer> players = new List<VideoPlayer> ();
}