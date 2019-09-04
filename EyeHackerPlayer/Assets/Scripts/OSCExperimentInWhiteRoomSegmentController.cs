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

    // public Transform staticBallsRoot;
    // public GameObject ballPrefab;

    [SerializeField] private uOscServer server;

    public OSCBallInfoSender infoSender;

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

                    // for (int i = 0; i < 30; i++)
                    // {
                    //     Vector3 ballPos = Random.onUnitSphere * 9.0f;
                    //     if (i < 10)
                    //     {
                    //         while (ballPos.z < 0.0f || ballPos.y > 9.0f * Mathf.Sin (Mathf.PI / 18.0f) || ballPos.y < 9.0f * Mathf.Sin (-Mathf.PI / 18.0f) || ballPos.x < -9.0f || ballPos.x > 9.0f * Mathf.Sin (-Mathf.PI / 6.0f))
                    //         {
                    //             ballPos = Random.onUnitSphere * 9.0f;
                    //         }
                    //     }
                    //     else if (i < 20)
                    //     {
                    //         while (ballPos.z < 0.0f || ballPos.y > 9.0f * Mathf.Sin (Mathf.PI / 18.0f) || ballPos.y < 9.0f * Mathf.Sin (-Mathf.PI / 18.0f) || ballPos.x < 9.0f * Mathf.Sin (Mathf.PI / 6.0f) || ballPos.x > 9.0f)
                    //         {
                    //             ballPos = Random.onUnitSphere * 9.0f;
                    //         }
                    //     }
                    //     else
                    //     {
                    //         while (ballPos.z < 9.0 * Mathf.Cos (Mathf.PI / 4.0f) || ballPos.y > 9.0f * Mathf.Sin (Mathf.PI / 18.0f) || ballPos.y < 9.0f * Mathf.Sin (-Mathf.PI / 18.0f) || ballPos.x < 9.0f * Mathf.Sin (-Mathf.PI / 6.0f) || ballPos.x > 9.0f * Mathf.Sin (Mathf.PI / 6.0f))
                    //         {
                    //             ballPos = Random.onUnitSphere * 9.0f;
                    //         }
                    //     }
                    //     GameObject obj = (GameObject) Instantiate (ballPrefab, ballPos, Quaternion.identity);
                    //     obj.transform.parent = staticBallsRoot;
                    // }
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

                        // Vector3 ballPos = Random.onUnitSphere * 9.0f;
                        // switch (i)
                        // {
                        //     // left
                        //     case 0:
                        //     case 1:
                        //         while (ballPos.z < 0.0f || ballPos.y > 9.0f * Mathf.Sin (Mathf.PI / 12.0f) || ballPos.y < 9.0f * Mathf.Sin (-Mathf.PI / 12.0f) || ballPos.x < -9.0f || ballPos.x > 9.0f * Mathf.Sin (-Mathf.PI / 6.0f))
                        //         {
                        //             ballPos = Random.onUnitSphere * 9.0f;
                        //         }
                        //         alphaModifier.spheres[i].transform.position = ballPos;
                        //         break;
                        //         // right
                        //     case 2:
                        //     case 3:
                        //         while (ballPos.z < 0.0f || ballPos.y > 9.0f * Mathf.Sin (Mathf.PI / 12.0f) || ballPos.y < 9.0f * Mathf.Sin (-Mathf.PI / 12.0f) || ballPos.x < 9.0f * Mathf.Sin (Mathf.PI / 6.0f) || ballPos.x > 9.0f)
                        //         {
                        //             ballPos = Random.onUnitSphere * 9.0f;
                        //         }
                        //         alphaModifier.spheres[i].transform.position = ballPos;
                        //         break;
                        //         // center
                        //     case 4:
                        //     case 5:
                        //         while (ballPos.z < 9.0 * Mathf.Cos (Mathf.PI / 6.0f) || ballPos.y > 9.0f * Mathf.Sin (Mathf.PI / 12.0f) || ballPos.y < 9.0f * Mathf.Sin (-Mathf.PI / 12.0f) || ballPos.x < 9.0f * Mathf.Sin (-Mathf.PI / 6.0f) || ballPos.x > 9.0f * Mathf.Sin (Mathf.PI / 6.0f))
                        //         {
                        //             ballPos = Random.onUnitSphere * 9.0f;
                        //         }
                        //         alphaModifier.spheres[i].transform.position = ballPos;
                        //         break;
                        //     default:
                        //         Debug.Log ("Out of Range.");
                        //         break;
                        // }
                    }
                    alphaModifier.isSelected = false;
                    alphaModifier.selectedBall = null;
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
                    infoSender.SendInfo ();
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