using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using UnityEngine.UI;

public class OSCExperimentInWhiteRoomSegmentController : MonoBehaviour
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
    public GameObject dot;

    public CreateBallPos createBallPos;

    [SerializeField] private uOscServer server;

    public OSCBallInfoSender infoSender;

    private int index = 0;

    private const int TRIAL_NUMBER = 48 * 3;

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
                    startSegmentZero (message);
                    break;
                case 1:
                    startSegmentOne (message);
                    break;
                case 2:
                    startSegmentTwo (message);
                    break;
                case 3:
                    startSegmentThree (message);
                    break;
                case 4:
                    startSegmentFour (message);
                    break;
                case 5:
                    startSegmentFive (message);
                    break;
                case 6:
                    startSegmentSix (message);
                    break;
                default:
                    Debug.Log ("Invalid Segment Number.");
                    break;
            }
        }
    }

    void startSegmentZero (Message message)
    {
        sphere.SetActive (true);
        room.SetActive (false);

        canvas.SetActive (true);
        slider.SetActive (false);
        buttons.SetActive (false);
        text.text = "Ready: No." + message.values[1].ToString ();

        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive (false);
        }
        dot.SetActive (false);

        alphaObject.SetActive (false);
        alphaModifier.isSelected = false;
    }

    void startSegmentOne (Message message)
    {
        string direction = message.values[1].ToString ();
        string speed = message.values[2].ToString ();
        float areaMin = System.Convert.ToSingle (message.values[3]);
        float areaMax = System.Convert.ToSingle (message.values[4]);

        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive (true);
            dot.SetActive (true);
        }

        if (direction != "CENTER")
        {
            string sentence1 = "";
            string sentence2 = "";

            switch (speed)
            {
                case "FAST":
                    sentence1 = "素早く";
                    dot.SetActive (false);
                    break;
                case "SLOW":
                    sentence1 = "ゆっくり";
                    // show only one arrow
                    for (int i = 0; i < arrows.Length; i++)
                    {
                        if (i != 0)
                        {
                            arrows[i].SetActive (false);
                        }
                        dot.SetActive (false);
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
                    // case "UP":
                    //     sentence2 = "上を";
                    //     for (int i = 0; i < arrows.Length; i++)
                    //     {
                    //         Vector3 rot = arrows[i].transform.rotation.eulerAngles;
                    //         rot.z = 90.0f;
                    //         arrows[i].transform.rotation = Quaternion.Euler (rot);
                    //     }
                    //     break;
                    // case "DOWN":
                    //     sentence2 = "下を";
                    //     for (int i = 0; i < arrows.Length; i++)
                    //     {
                    //         Vector3 rot = arrows[i].transform.rotation.eulerAngles;
                    //         rot.z = 270.0f;
                    //         arrows[i].transform.rotation = Quaternion.Euler (rot);
                    //     }
                    //     break;
                default:
                    Debug.Log ("Undefined phrase.");
                    break;
            }
            text.text = "トリガーを引いて開始します．\n\n+が消えたら\n" + sentence1 + sentence2 + "向いてください．";
        }
        // direction == "CENTER"
        else
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].SetActive (false);
            }
            text.text = "トリガーを引いて開始します．\n\n中央を\n向き続けてください．";
        }

        alphaModifier.min = areaMin;
        alphaModifier.max = areaMax;
    }

    void startSegmentTwo (Message message)
    {
        sphere.SetActive (false);
        room.SetActive (true);

        text.text = "";
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive (false);
        }
        dot.SetActive (false);
        canvas.SetActive (false);

        startPoint.SetActive (true);
        startPoint.GetComponentInChildren<CountDownController> ().CountDown ();

        createBallPos.DestroyAll ();
        createBallPos.SetBallPositions (index);
        if (index < TRIAL_NUMBER) index += 1;

        alphaObject.SetActive (true);
        alphaModifier.center = new Vector3 (0.0f, 0.0f, 0.0f);
        alphaModifier.selectedBall = null;
    }

    void startSegmentThree (Message message)
    {
        startPoint.SetActive (false);
    }

    void startSegmentFour (Message message)
    {
        sphere.SetActive (true);
        room.SetActive (false);

        alphaObject.SetActive (false);
        infoSender.SendInfo ();

        canvas.SetActive (true);
        buttons.SetActive (true);
        yes.image.color = Color.gray;
        no.image.color = Color.gray;
        text.text = "変化があったかどうか\nタッチパッドの左右で選択し\nトリガーで決定してください．";
    }

    void startSegmentFive (Message message)
    {
        buttons.SetActive (false);
        slider.SetActive (true);
        slider.GetComponent<Slider> ().value = 4;
        text.text = "YES/NOの選択の確信度を\nタッチパッドの左右で選択し\nトリガーで決定してください．";
    }

    void startSegmentSix (Message message)
    {
        buttons.SetActive (false);
        slider.SetActive (false);
        text.text = "一旦1分以上休憩してください．\n再開したいタイミングで\n声をかけてください．";
    }
}