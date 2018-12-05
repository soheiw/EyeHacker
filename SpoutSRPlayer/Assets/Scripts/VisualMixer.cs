using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VisualMixer : MonoBehaviour {
    private Renderer renderer;
    private long latestTimeStamp;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<Renderer>();
        OSCHandler.Instance.CreateServer("SRCamera", 9999);
	}
	
	// Update is called once per frame
	void Update ()
    {
        OSCReceive();
	}

    private void OSCReceive()
    {
        OSCHandler.Instance.UpdateLogs();
        foreach (KeyValuePair<string, ServerLog> item in OSCHandler.Instance.Servers)
        {
            if(item.Value.packets.Count == 0)
            {
                continue;
            }
            int latestPacketIndex = item.Value.packets.Count - 1;
            if (this.latestTimeStamp == item.Value.packets[latestPacketIndex].TimeStamp)
            {
                continue;
            }
            this.latestTimeStamp = item.Value.packets[latestPacketIndex].TimeStamp;

            var address = item.Value.packets[latestPacketIndex].Address;
            float message = (float)item.Value.packets[latestPacketIndex].Data[0];
            Debug.Log("Receive : "
                  + item.Value.packets[latestPacketIndex].TimeStamp
                  + " / "
                  + address
                  + " / "
                  + message);

            switch (address)
            {
                case "/blend1":
                    Debug.Log(message);
                    renderer.material.SetFloat("_BlendTexRatio1", message);
                    break;

                case "/blend2":
                    Debug.Log(message);
                    renderer.material.SetFloat("_BlendTexRatio2", message);
                    break;

                case "/blend3":
                    Debug.Log(message);
                    renderer.material.SetFloat("_BlendTexRatio3", message);
                    break;

                case "/blendall":
                    Debug.Log(message);
                    float message1 = (float)item.Value.packets[latestPacketIndex].Data[0];
                    float message2 = (float)item.Value.packets[latestPacketIndex].Data[1];
                    float message3 = (float)item.Value.packets[latestPacketIndex].Data[2];
                    renderer.material.SetFloat("_BlendTexRatio1", (float)item.Value.packets[latestPacketIndex].Data[0]);
                    renderer.material.SetFloat("_BlendTexRatio2", (float)item.Value.packets[latestPacketIndex].Data[1]);
                    renderer.material.SetFloat("_BlendTexRatio3", (float)item.Value.packets[latestPacketIndex].Data[2]);
                    break;

                case "/blendcheck":
                    Debug.Log(message);
                    renderer.material.SetFloat("_BlendTexRatio1", (float)item.Value.packets[latestPacketIndex].Data[0]);
                    renderer.material.SetFloat("_BlendTexRatio2", (float)item.Value.packets[latestPacketIndex].Data[1]);
                    renderer.material.SetFloat("_BlendTexRatio3", (float)item.Value.packets[latestPacketIndex].Data[2]);
                    renderer.material.SetFloat("_BlendTexRatio4", (float)item.Value.packets[latestPacketIndex].Data[3]);
                    break;

                default:
                    Debug.Log("Address Error: " + address);
                    break;
            }
        }
    }
}
