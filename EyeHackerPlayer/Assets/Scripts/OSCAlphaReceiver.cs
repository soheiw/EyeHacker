using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;

public class OSCAlphaReceiver : MonoBehaviour
{
    public GameObject[] spheres;
    public float[] alphas;

    public Color color = new Color (0.0f, 165.0f / 255.0f, 255.0f / 255.0f, 1.0f);
    [SerializeField] private uOscServer server;

    // Start is called before the first frame update
    void Start ()
    {
        alphas = new float[spheres.Length];
        for (int i = 0; i < spheres.Length; i++)
        {
            alphas[i] = 0.0f;
        }

        if (!server)
        {
            UnityEngine.Debug.Log ("uOSCserver is not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/alpha")
        {
            int num = System.Convert.ToInt32 (message.values[1]) - 1;
            if (num == -1) return;
            alphas[num] = System.Convert.ToSingle (message.values[0]);
            spheres[num].GetComponent<Renderer> ().material.color = new Vector4 (color.r, color.g, color.b, alphas[num]);
        }
    }
}