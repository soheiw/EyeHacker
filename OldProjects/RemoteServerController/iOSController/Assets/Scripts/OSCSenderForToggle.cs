using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCSenderForToggle : MonoBehaviour
{
    public uOscClient client;
    public string address;

    public bool onToOff = true;

    public void OnToggleChanged (Toggle toggle)
    {
        if (toggle.isOn)
        {
            Debug.Log ("Send" + address);
            client.Send (address, 1);
        }
        else
        {
            if (!onToOff) return;
            
            Debug.Log ("Send" + address);
            client.Send (address, 0);
        }
    }
}