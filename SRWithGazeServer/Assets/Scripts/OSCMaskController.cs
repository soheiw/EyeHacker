using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCMaskController : MonoBehaviour
{
    public GameObject mask;

    public Material recorded;
    public Material realtime;
    public string playerName;

    [SerializeField] private uOscServer server;
    private MeshRenderer meshRenderer;
    public Vector3 originalScale;

    // Use this for initialization
    void Start ()
    {
        meshRenderer = GetComponent<MeshRenderer> ();
        server = FindObjectOfType<uOscServer> ();
        if (!server) return;
        server.onDataReceived.AddListener (OnDataReceived);
        originalScale = mask.transform.localScale;
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/server/" + playerName + "/realtimemask")
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

        if (message.address == "/server/" + playerName + "/maskscale")
        {
            var ratio = (float) message.values[0];
            mask.transform.localScale = originalScale * ratio;
        }
    }
}