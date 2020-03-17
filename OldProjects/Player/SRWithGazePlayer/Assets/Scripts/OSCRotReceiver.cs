using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCRotReceiver : MonoBehaviour
{
    public uOscServer server;
    public GameObject sphere;

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
        if (message.address == "/player/sphererot")
        {
            float r = System.Convert.ToSingle (message.values[0]);
            float p = System.Convert.ToSingle (message.values[1]);
            float y = System.Convert.ToSingle (message.values[2]);
            sphere.transform.rotation = Quaternion.Euler (p, y, r);
        }
    }
}