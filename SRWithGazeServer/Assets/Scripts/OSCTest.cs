using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCTest : MonoBehaviour
{
    public string recordVideoName;
    public string playVideoName;
    public float ratio = 0.2f;
    public float maskScale = 1.2f;

    private uOscClient client;

    // Use this for initialization
    void Start ()
    {
        client = GetComponent<uOscClient> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.R))
        {
            client.Send ("/ndiserver/Recorder1/startrecording", recordVideoName);
        }

        if (Input.GetKeyDown (KeyCode.T))
        {
            client.Send ("/ndiserver/Recorder1/stoprecording");
        }

        if (Input.GetKeyDown (KeyCode.Space))
        {
            client.Send ("/ndiserver/Player1/startplaying", playVideoName);
        }

        if (Input.GetKeyDown (KeyCode.P))
        {
            client.Send ("/ndiserver/Player1/startplaying", playVideoName, ratio);
        }

        if (Input.GetKeyDown (KeyCode.L))
        {
            client.Send ("/ndiserver/Player1/stopplaying");
        }

        if (Input.GetKeyDown (KeyCode.A))
        {
            client.Send ("/ndiserver/Player1/getvideonames", "127.0.0.1");
        }

        if (Input.GetKeyDown (KeyCode.Y))
        {
            client.Send ("/ndiserver/Player1/maskscale", maskScale);
        }
    }
}