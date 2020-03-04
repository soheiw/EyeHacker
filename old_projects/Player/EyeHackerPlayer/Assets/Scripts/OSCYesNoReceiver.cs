using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCYesNoReceiver : MonoBehaviour
{
    public Button yes;
    public Button no;
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
        if (message.address == "/player/yesno")
        {
            int value = System.Convert.ToInt32 (message.values[0]);

            if (value == 1)
            {
                // Debug.Log ("yes");
                yes.image.color = Color.red;
                no.image.color = Color.gray;
            }
            else if (value == 0)
            {
                // Debug.Log ("No");
                yes.image.color = Color.gray;
                no.image.color = Color.red;
            }
        }
    }
}