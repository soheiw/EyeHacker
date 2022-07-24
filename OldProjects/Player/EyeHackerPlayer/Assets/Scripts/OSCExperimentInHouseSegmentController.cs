﻿using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCExperimentInHouseSegmentController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject slider;
    public GameObject buttons;
    public Button yes;
    public Button no;
    public Text text;

    public GameObject sphere;
    public GameObject room;

    public GameObject startPoint;

    public GameObject alphaObject;
    public OSCAlphaModifier alphaModifier;

    public GameObject[] arrows;

    [SerializeField] private uOscServer server;

    // Start is called before the first frame update
    void Start ()
    {
        if (!server)
        {
            UnityEngine.Debug.Log ("uOSCserver is not set");
            return;
        }
        server.onDataReceived.AddListener (OnDataReceived);
    }

    void OnDataReceived (Message message)
    {
        if (message.address == "/player/segment")
        {
            int number = System.Convert.ToInt32 (message.values[0]);
            switch (number)
            {
                case 0:
                    sphere.SetActive (true);
                    room.SetActive (false);
                    canvas.SetActive (true);
                    slider.SetActive (false);
                    buttons.SetActive (false);
                    alphaObject.SetActive (false);
                    text.text = "Ready: No." + message.values[1].ToString ();
                    break;
                case 1:
                    string direction = message.values[1].ToString ();
                    string speed = message.values[2].ToString ();

                    for (int i = 0; i < arrows.Length; i++)
                    {
                        arrows[i].SetActive (true);
                    }

                    if (direction != "CENTER")
                    {
                        string sentence1 = "";
                        string sentence2 = "";

                        switch (speed)
                        {
                            case "FAST":
                                sentence1 = "素早く";
                                break;
                            case "SLOW":
                                sentence1 = "ゆっくり";
                                for (int i = 0; i < arrows.Length; i++)
                                {
                                    if (i != 0)
                                    {
                                        arrows[i].SetActive (false);
                                    }
                                }
                                break;
                            default:
                                Debug.Log ("Undefined phrase.");
                                break;
                        }

                        switch (direction)
                        {
                            case "LEFT":
                                sentence2 = "左を";
                                for (int i = 0; i < arrows.Length; i++)
                                {
                                    Vector3 rot = arrows[i].transform.rotation.eulerAngles;
                                    rot.z = 180.0f;
                                    arrows[i].transform.rotation = Quaternion.Euler (rot);
                                }
                                break;
                            case "RIGHT":
                                sentence2 = "右を";
                                for (int i = 0; i < arrows.Length; i++)
                                {
                                    Vector3 rot = arrows[i].transform.rotation.eulerAngles;
                                    rot.z = 0.0f;
                                    arrows[i].transform.rotation = Quaternion.Euler (rot);
                                }
                                break;
                            case "UP":
                                sentence2 = "上を";
                                for (int i = 0; i < arrows.Length; i++)
                                {
                                    Vector3 rot = arrows[i].transform.rotation.eulerAngles;
                                    rot.z = 90.0f;
                                    arrows[i].transform.rotation = Quaternion.Euler (rot);
                                }
                                break;
                            case "DOWN":
                                sentence2 = "下を";
                                for (int i = 0; i < arrows.Length; i++)
                                {
                                    Vector3 rot = arrows[i].transform.rotation.eulerAngles;
                                    rot.z = 270.0f;
                                    arrows[i].transform.rotation = Quaternion.Euler (rot);
                                }
                                break;
                            default:
                                Debug.Log ("Undefined phrase.");
                                break;
                        }
                        text.text = "\n+が消えたら\n" + sentence1 + sentence2 + "向いてください．\nトリガーを引いて開始します．";
                    }
                    else
                    {
                        for (int i = 0; i < arrows.Length; i++)
                        {
                            arrows[i].SetActive (false);
                        }
                        text.text = "\n中央を\n向き続けてください．\nトリガーを引いて開始します．";
                    }
                    break;
                case 2:
                    sphere.SetActive (false);
                    room.SetActive (true);
                    text.text = "";
                    startPoint.SetActive (true);
                    startPoint.GetComponentInChildren<CountDownController> ().CountDown ();
                    for (int i = 0; i < arrows.Length; i++)
                    {
                        arrows[i].SetActive (false);
                    }
                    canvas.SetActive (false);

                    alphaObject.SetActive (true);
                    for (int i = 0; i < alphaModifier.spheres.Length; i++)
                    {
                        alphaModifier.alphas[i] = 0.0f;
                        alphaModifier.spheres[i].GetComponent<Renderer> ().material.color = new Vector4 (alphaModifier.color.r, alphaModifier.color.g, alphaModifier.color.b, 0.0f);
                    }
                    break;
                case 3:
                    startPoint.SetActive (false);
                    break;
                case 4:
                    sphere.SetActive (true);
                    room.SetActive (false);
                    canvas.SetActive (true);
                    buttons.SetActive (true);
                    yes.image.color = Color.gray;
                    no.image.color = Color.gray;
                    text.text = "変化があったかどうか\nタッチパッドの左右で選択し\nトリガーで決定してください．";

                    alphaObject.SetActive (false);
                    break;
                case 5:
                    slider.SetActive (true);
                    buttons.SetActive (false);
                    slider.GetComponent<Slider> ().value = 4;
                    text.text = "YES/NOの選択の確信度を\nタッチパッドの左右で選択し\nトリガーで決定してください．";

                    alphaObject.SetActive (false);
                    break;
                default:
                    Debug.Log ("Invalid Segment Number.");
                    break;
            }
        }
    }
}