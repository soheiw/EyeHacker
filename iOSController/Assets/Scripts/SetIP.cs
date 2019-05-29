using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class SetIP : MonoBehaviour
{
    public uOscClient client;

    public void OnEndEdit (string text)
    {
        client.address = text;
    }
}