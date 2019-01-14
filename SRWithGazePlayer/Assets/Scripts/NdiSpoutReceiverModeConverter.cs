using System.Collections;
using System.Collections.Generic;
using Klak.Ndi;
using Klak.Spout;
using UnityEngine;

public class NdiSpoutReceiverModeConverter : MonoBehaviour
{
    public bool isNDI;
    [SerializeField] private NdiReceiver[] ndiReceivers;
    [SerializeField] private SpoutReceiver[] spoutReceivers;

    // Use this for initialization
    void Start ()
    {
        isNDI = true;

        // 重いので基本的にはコメントアウトしてinspectorで操作
        ndiReceivers = GameObject.FindObjectsOfType<NdiReceiver> ();
        spoutReceivers = GameObject.FindObjectsOfType<SpoutReceiver> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.M))
        {
            isNDI = !isNDI;

            if (isNDI)
            {
                SetEnabledForAllNdiReceivers (ndiReceivers, true);
                SetEnabledForAllSpoutReceivers (spoutReceivers, false);
            }
            else
            {
                SetEnabledForAllNdiReceivers (ndiReceivers, false);
                SetEnabledForAllSpoutReceivers (spoutReceivers, true);
            }
        }
    }

    private void SetEnabledForAllNdiReceivers (NdiReceiver[] components, bool b)
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].enabled = b;
        }
    }

    private void SetEnabledForAllSpoutReceivers (SpoutReceiver[] components, bool b)
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].enabled = b;
        }
    }
}