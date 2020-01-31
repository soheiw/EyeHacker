using System.Collections;
using System.Collections.Generic;
using Klak.Spout;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTextureSize : MonoBehaviour
{
    public Texture[] textures;
    public Material material;
    public SpoutReceiver spoutReceiver;
    public TextMeshProUGUI text;

    private int num;

    // Start is called before the first frame update
    void Start ()
    {
        num = 0;
        Texture first = textures[0];
        material.SetTexture ("_MainTex", first);
        text.text = "Texture: " + first.name;
        spoutReceiver.targetTexture = (RenderTexture) first;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.T))
        {
            num++;
            Texture selected = textures[num % textures.Length];
            material.SetTexture ("_MainTex", selected);
            text.text = "Texture: " + selected.name;
            spoutReceiver.targetTexture = (RenderTexture) selected;
        }
    }
}