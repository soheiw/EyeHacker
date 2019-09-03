using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCExperimentSegmentController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject slider;
    public Text text;

    public GameObject sphere;
    public GameObject room;

    public GameObject startPoint;

    public GameObject alphaObject;
    public OSCAlphaReceiver alphaReceiver;

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
                    alphaObject.SetActive (false);
                    text.text = "Ready: No." + message.values[1].ToString ();
                    break;
                case 1:
                    string direction = message.values[1].ToString ();
                    string speed = message.values[2].ToString ();
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
                                break;
                            default:
                                Debug.Log ("Undefined phrase.");
                                break;
                        }

                        switch (direction)
                        {
                            case "LEFT":
                                sentence2 = "左を";
                                break;
                            case "RIGHT":
                                sentence2 = "右を";
                                break;
                            case "UP":
                                sentence2 = "上を";
                                break;
                            case "DOWN":
                                sentence2 = "下を";
                                break;
                            default:
                                Debug.Log ("Undefined phrase.");
                                break;
                        }
                        text.text = "+が消えたら\n" + sentence1 + sentence2 + "\n向いてください．\n変化が起きたらトリガーを\n引いてください．";
                    }
                    else
                    {
                        text.text = "\n中央を\n向き続けてください．\n変化が起きたらトリガーを\n引いてください．";
                    }
                    break;
                case 2:
                    sphere.SetActive (false);
                    room.SetActive (true);
                    text.text = "";
                    startPoint.SetActive (true);
                    startPoint.GetComponentInChildren<CountDownController> ().CountDown ();
                    canvas.SetActive (false);

                    alphaObject.SetActive (true);
                    for (int i = 0; i < alphaReceiver.alphas.Length; i++)
                    {
                        alphaReceiver.alphas[i] = 0.0f;
                        alphaReceiver.spheres[i].GetComponent<Renderer> ().material.color = new Vector4 (alphaReceiver.color.r, alphaReceiver.color.g, alphaReceiver.color.b, 0.0f);
                    }
                    break;
                case 3:
                    startPoint.SetActive (false);
                    break;
                case 4:
                    sphere.SetActive (true);
                    room.SetActive (false);
                    canvas.SetActive (true);
                    slider.SetActive (true);
                    slider.GetComponent<Slider> ().value = 4;
                    text.text = "変化の有無を選んだときの\n確信度をタッチパッドの\n左右を押して選択してください．";

                    alphaObject.SetActive (false);
                    break;
                default:
                    Debug.Log ("Invalid Segment Number.");
                    break;
            }
        }
    }
}