using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCRotationReceiver : MonoBehaviour
{
    public GameObject sphere;
    [SerializeField] private uOscServer server;

    // Start is called before the first frame update
    void Start ()
    {
        if (!server)
        {
            UnityEngine.Debug.Log ("uOSCserver is not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);
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