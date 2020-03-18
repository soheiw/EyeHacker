using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCConfidenceReceiver : MonoBehaviour
{
    public Slider slider;
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
        if (message.address == "/player/confidence")
        {
            slider.value = System.Convert.ToInt32 (message.values[0]);
        }
    }
}