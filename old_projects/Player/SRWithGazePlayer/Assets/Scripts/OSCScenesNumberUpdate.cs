using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCScenesNumberUpdate : MonoBehaviour
{
    private RayManager rayManager;
    private DetermineGazeByHMD determineGazeByHMD;

    [SerializeField] private uOscServer server;

    public Text text;

    // Use this for initialization
    void Start ()
    {
        if (!server)
        {
            UnityEngine.Debug.Log ("OSCserver not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);

    }

    // Update is called once per frame
    void Update ()
    {

    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/scenesnumber")
        {
            var sentence = message.values[0].ToString ();
            text.text = "No. " + sentence;
        }
    }
}