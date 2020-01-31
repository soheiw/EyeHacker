using System.Collections;
using System.Collections.Generic;
using TMPro;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCSceneNumUpdate : MonoBehaviour
{
    private TextMeshProUGUI text;

    [SerializeField] private uOscServer server;

    // Start is called before the first frame update
    void Start ()
    {
        text = GetComponent<TextMeshProUGUI> ();

        if (!server)
        {
            UnityEngine.Debug.Log ("uOSCserver is not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/scenenumber")
        {
            var num = message.values[0].ToString ();
            text.text = "No. " + num;
        }
    }
}