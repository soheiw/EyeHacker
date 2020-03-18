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
        float baseline = Mathf.Lerp(10.0f, 200.0f, v);
        Debug.Log (baseline);
        client.Send (address, baseline);
    }
}