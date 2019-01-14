using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGHandler : MonoBehaviour {
    public Vector3 pos = new Vector3(0, -1.5f, 5.0f);
    public float roty = 180.0f;
    public float scale = 1.0f;
    public int objectNum = 0;
    public int objectType = 0;

    [Header("Prefab")]
    public GameObject[] CGPrefabs;
    public GameObject UntransferredPrefeb;

    private GameObject[] copiedObjects;
    private GameObject objUntransferred;

    private long latestTimeStamp;

    // Use this for initialization
    void Start () {
        OSCHandler.Instance.CreateServer("CGHandler", 9998);
        copiedObjects = new GameObject[100];
    }
	
	// Update is called once per frame
	void Update () {
        OSCReceive();
	}

    private void OSCReceive()
    {
        OSCHandler.Instance.UpdateLogs();
        foreach (KeyValuePair<string, ServerLog> item in OSCHandler.Instance.Servers)
        {
            if (item.Value.packets.Count == 0)
            {
                continue;
            }
            int latestPacketIndex = item.Value.packets.Count - 1;
            if (this.latestTimeStamp == item.Value.packets[latestPacketIndex].TimeStamp)
            {
                continue;
            }
            //this.latestTimeStamp = item.Value.packets[latestPacketIndex].TimeStamp;

            var address = item.Value.packets[latestPacketIndex].Address;
            float message = (float)item.Value.packets[latestPacketIndex].Data[0]; // used for classifying the signal
            Debug.Log("Receive : "
                  + item.Value.packets[latestPacketIndex].TimeStamp
                  + " / "
                  + address
                  + " / "
                  + message);


            if(message == 2) // make an object which will be transferred
            {
                float message1, message2, message3, message4, message5, message6;
                int message7, message8;
                this.latestTimeStamp = item.Value.packets[latestPacketIndex].TimeStamp;

                switch (address)
                {
                    case "/dup":
                        /* MEANING
                         * 2~4: r-theta-phi coordinate in range[0,10],[0,360],[0,360]
                         * 5: Yaw-directional rotation in [0,360]
                         * 6: Scale in [1,11]
                         * 7: Selected object
                         * 8: Original object type (The contents can be changed by Unity.)*/

                        message1 = (float)item.Value.packets[latestPacketIndex].Data[0];
                        message2 = (float)item.Value.packets[latestPacketIndex].Data[1];
                        message3 = (float)item.Value.packets[latestPacketIndex].Data[2];
                        message4 = (float)item.Value.packets[latestPacketIndex].Data[3];
                        message5 = (float)item.Value.packets[latestPacketIndex].Data[4];
                        message6 = (float)item.Value.packets[latestPacketIndex].Data[5];
                        message7 = (int)item.Value.packets[latestPacketIndex].Data[6];
                        message8 = (int)item.Value.packets[latestPacketIndex].Data[7];
                        pos.x = message2 * Mathf.Cos(message3) * Mathf.Sin(message4);
                        pos.y = message2 * Mathf.Sin(message3);
                        pos.z = message2 * Mathf.Cos(message3) * Mathf.Cos(message4);
                        roty = message5;
                        scale = message6;
                        objectNum = message7;
                        objectType = message8;

                        Quaternion rot = Quaternion.AngleAxis(roty, Vector3.up);

                        if (copiedObjects[objectNum] != null)
                        {
                            Destroy(copiedObjects[objectNum]);

                        }
                        copiedObjects[objectNum] = Instantiate(CGPrefabs[objectType], pos, rot);
                        copiedObjects[objectNum].transform.localScale = new Vector3(scale, scale, scale);
                        copiedObjects[objectNum].layer = 8;
                        break;

                    case "/del":
                        /* MEANING
                         * 7: Selected Object*/
                        message7 = (int)item.Value.packets[latestPacketIndex].Data[1];
                        objectNum = message7;

                        if (copiedObjects[objectNum] != null)
                        {
                            Destroy(copiedObjects[objectNum]);

                        }
                        break;

                    case "/mov":
                        /* MEANING
                         * 2~4: r-theta-phi coordinate in range[0,10],[0,360],[0,360]
                         * 5: Yaw-directional rotation in [0,360]
                         * 6: Scale in [1,11]
                         * 7: Selected object
                         * 8: Original object type (The contents can be changed by Unity.)*/
                        message1 = (float)item.Value.packets[latestPacketIndex].Data[0];
                        message2 = (float)item.Value.packets[latestPacketIndex].Data[1];
                        message3 = (float)item.Value.packets[latestPacketIndex].Data[2];
                        message4 = (float)item.Value.packets[latestPacketIndex].Data[3];
                        message5 = (float)item.Value.packets[latestPacketIndex].Data[4];
                        message6 = (float)item.Value.packets[latestPacketIndex].Data[5];
                        message7 = (int)item.Value.packets[latestPacketIndex].Data[6];
                        message8 = (int)item.Value.packets[latestPacketIndex].Data[7];
                        pos.x = message2 * Mathf.Cos(message3) * Mathf.Sin(message4);
                        pos.y = message2 * Mathf.Sin(message3);
                        pos.z = message2 * Mathf.Cos(message3) * Mathf.Cos(message4);
                        roty = message5;
                        scale = message6;
                        objectNum = message7;
                        objectType = message8;

                        if (copiedObjects[objectNum] != null)
                        {
                            copiedObjects[objectNum].transform.position = pos;
                            copiedObjects[objectNum].transform.rotation = Quaternion.AngleAxis(roty, Vector3.up);
                            copiedObjects[objectNum].transform.localScale = new Vector3(scale, scale, scale);
                        }
                        break;

                    default:
                        Debug.Log("Address Error: " + address);
                        break;
                }
            }
            else if(message == 3) // make an object which you don't want to transfer (Like Onomatope)
            {
                // Now this object will be placed in the transfered plane,
                // which will be distorted as Panoramic Transformation.
                float message1, message2, message3;
                this.latestTimeStamp = item.Value.packets[latestPacketIndex].TimeStamp;

                switch (address)
                {
                    case "/d2d":
                        message1 = (float)item.Value.packets[latestPacketIndex].Data[0];
                        message2 = (float)item.Value.packets[latestPacketIndex].Data[1];
                        message3 = (float)item.Value.packets[latestPacketIndex].Data[2];
                        pos = new Vector3(message2, message3, 5.0f);
                        roty = 180.0f;

                        Vector3 offset = new Vector3(47.07f, 0.37f, 0);
                        pos += offset;
                        Quaternion rot = Quaternion.AngleAxis(roty, Vector3.up);

                        if (objUntransferred != null)
                        {
                            Destroy(objUntransferred);
                        }
                        objUntransferred = Instantiate(UntransferredPrefeb, pos, rot);
                        Destroy(objUntransferred, 0.5f);
                        Debug.Log("Instantiate: " + objUntransferred.transform.position);
                        objUntransferred.layer = 8;
                        break;

                    case "/c2d":
                        if (objUntransferred != null)
                        {
                            Destroy(objUntransferred);
                        }
                        break;

                    case "/m2d":
                        message1 = (float)item.Value.packets[latestPacketIndex].Data[0];
                        message2 = (float)item.Value.packets[latestPacketIndex].Data[1];
                        message3 = (float)item.Value.packets[latestPacketIndex].Data[2];
                        pos = new Vector3(message2, message3, 5.0f);
                        roty = 180.0f;

                        if (objUntransferred != null)
                        {
                            objUntransferred.transform.position = pos;
                            objUntransferred.transform.rotation = Quaternion.AngleAxis(roty, Vector3.up);
                        }
                        break;

                    default:
                        Debug.Log("Address Error: " + address);
                        break;
                }
            }
            else
            {
                continue;
            }
        }
    }
}
