using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCFpsSender : MonoBehaviour
{
    [SerializeField] private uOscClient client;
    [SerializeField] private string address;

    private int frameCount;
    private float prevTime;

    public float fps;

    // Start is called before the first frame update
    void Start ()
    {
        if (!client)
        {
            UnityEngine.Debug.Log ("uOSCclient is not set");
            return;
        }

        frameCount = 0;
        prevTime = 0.0f;
        fps = 60.0f;
    }

    // Update is called once per frame
    void Update ()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            fps = frameCount / time;

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }

        client.Send (address, fps);
    }
}