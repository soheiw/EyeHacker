using System.Collections;
using System.Collections.Generic;
using Klak.Ndi;
using Klak.Spout;
using UnityEngine;

public class NdiSpoutSenderModeConverter : MonoBehaviour
{
    public bool isNDI;
    [SerializeField] private NdiSender[] ndiSenders;
    [SerializeField] private SpoutSender[] spoutSenders;

    // Use this for initialization
    void Start ()
    {
        isNDI = true;

        // 重いので基本的にはコメントアウトしてinspectorで操作
        ndiSenders = GameObject.FindObjectsOfType<NdiSender> ();
        spoutSenders = GameObject.FindObjectsOfType<SpoutSender> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.M))
        {
            isNDI = !isNDI;

            if (isNDI)
            {
                SetEnabledForAllNdiSenders (ndiSenders, true);
                SetEnabledForAllSpoutSenders (spoutSenders, false);
            }
            else
            {
                SetEnabledForAllNdiSenders (ndiSenders, false);
                SetEnabledForAllSpoutSenders (spoutSenders, true);
            }
        }
    }

    private void SetEnabledForAllNdiSenders (NdiSender[] components, bool b)
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].enabled = b;
        }
    }

    private void SetEnabledForAllSpoutSenders (SpoutSender[] components, bool b)
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].enabled = b;
        }
    }
}