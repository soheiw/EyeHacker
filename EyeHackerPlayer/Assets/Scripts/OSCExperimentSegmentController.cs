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
    public Material fromTD;
    public Material black;

    public GameObject startPoint;

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
                    sphere.GetComponent<Renderer> ().material = black;
                    canvas.SetActive (true);
                    slider.SetActive (false);
                    text.text = "Ready: No." + message.values[1].ToString ();
                    break;
                case 1:
                    string direction = message.values[1].ToString ();
                    if (direction != "CENTER")
                    {
                        string sentence = "";
                        switch (direction)
                        {
                            case "LEFT":
                                sentence = "左を";
                                break;
                            case "RIGHT":
                                sentence = "右を";
                                break;
                            case "UP":
                                sentence = "上を";
                                break;
                            case "DOWN":
                                sentence = "下を";
                                break;
                            default:
                                Debug.Log ("Undefined phrase.");
                                break;
                        }
                        text.text = "+が消えたら\nゆっくり" + sentence + "\n向いてください．\n変化が起きたらトリガーを\n引いてください．";
                    }
                    else
                    {
                        text.text = "\n中央を\n向き続けてください．\n変化が起きたらトリガーを\n引いてください．";
                    }
                    break;
                case 2:
                    sphere.GetComponent<Renderer> ().material = fromTD;
                    text.text = "";
                    startPoint.SetActive (true);
                    startPoint.GetComponentInChildren<CountDownController> ().CountDown ();
                    canvas.SetActive (false);
                    break;
                case 3:
                    startPoint.SetActive (false);
                    break;
                case 4:
                    sphere.GetComponent<Renderer> ().material = black;
                    canvas.SetActive (true);
                    slider.SetActive (true);
                    slider.GetComponent<Slider> ().value = 4;
                    text.text = "変化が起きたかどうかの\n確信度をタッチパッドの\n左右を押して選択してください．";
                    break;
                default:
                    Debug.Log ("Invalid Segment Number.");
                    break;
            }
        }
    }
}