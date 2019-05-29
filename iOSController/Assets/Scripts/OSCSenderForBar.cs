using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCSenderForBar : MonoBehaviour
{
    public uOscClient client;
    public string address;

    public void OnValueChanged (float v)
    {
        float baseline = 10 + v * 30;
        Debug.Log ("Send" + address);
        client.Send (address, baseline);
    }
}