using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SetActiveWithPlayer : MonoBehaviour
{
    public GameObject obj;
    public VideoPlayer player;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        obj.SetActive (player.isPlaying);
    }
}