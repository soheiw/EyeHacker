using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class SwitchMask : MonoBehaviour
{
    public Material recorded;
    public Material realtime;
    public string playerName;

    [SerializeField] private uOscServer server;
    private MeshRenderer meshRenderer;

    // Use this for initialization
    void Start ()
    {
        meshRenderer = GetComponent<MeshRenderer> ();
        server = FindObjectOfType<uOscServer> ();
        if (!server) server.onDataReceived.AddListener (OnDataReceived);
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/ndiserver/" + playerName + "/realtimemask")
        {
            var state = (float) message.values[0];
            if (state != 0)
            {
                meshRenderer.material = realtime;
            }
            else
            {
                meshRenderer.material = recorded;
            }
        }
    }
}