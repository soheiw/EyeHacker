using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCReceiverForPanel : MonoBehaviour
{
    public uOscServer server;
    public GameObject[] toggles;
    public string address;

    // Use this for initialization
    void Start ()
    {
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == address)
        {
            if ((int) message.values[0] == 0)
            {
                for (int i = 0; i < toggles.Length; i++)
                {
                    ToggleInteractiveController interact = toggles[i].GetComponent<ToggleInteractiveController> ();
                    interact.enabled = true;
                    Debug.Log ("neko");
                    toggles[i].GetComponent<Toggle> ().interactable = true;
                }
            }
            else if ((int) message.values[0] == 1)
            {
                for (int i = 0; i < toggles.Length; i++)
                {
                    ToggleInteractiveController interact = toggles[i].GetComponent<ToggleInteractiveController> ();
                    interact.enabled = false;
                    Debug.Log ("nyan");
                    toggles[i].GetComponent<Toggle> ().interactable = false;
                }
            }
        }
    }
}