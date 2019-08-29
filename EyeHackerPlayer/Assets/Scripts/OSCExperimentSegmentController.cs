using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCExperimentSegmentController : MonoBehaviour
{
    public Text text;
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
        if (message.address == "/player/segment")
        {
            int number = System.Convert.ToInt32 (message.values[0]);
            switch (number)
            {
                case 0:
                    text.text = "No." + message.values[1].ToString ();
                    break;
                case 1:
                    text.text = message.values[1].ToString ();
                    break;
                case 2:
                    text.text = "";
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    Debug.Log ("Invalid Segment Number.");
                    break;
            }
        }
    }
}