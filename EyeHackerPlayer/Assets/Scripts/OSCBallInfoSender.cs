using System.Collections;
using System.Collections.Generic;
using uOSC;
using UnityEngine;
using ViveSR.anipal.Eye;

public class OSCBallInfoSender : MonoBehaviour
{
    [SerializeField] private uOscClient client;
    [SerializeField] private string address;

    [SerializeField] private OSCAlphaModifier alphaModifier;
    [SerializeField] private DataWrite dataWrite;

    // Start is called before the first frame update
    void Start ()
    {
        if (!client)
        {
            UnityEngine.Debug.Log ("uOSCclient is not set");
            return;
        }
    }

    public void SendInfo ()
    {
        // Vector3 cameraRot = 9.0f * Camera.main.transform.forward;
        // Vector3 cameraRot = Camera.main.transform.rotation.eulerAngles;
        Vector3 headPos = dataWrite.headPosition;
        Vector3 eyeDir = dataWrite.eyeDir;

        if (alphaModifier.selectedBall != null)
        {
            client.Send (address, 
            alphaModifier.center.x, alphaModifier.center.y, alphaModifier.center.z,alphaModifier.selectedBall.transform.position.x, alphaModifier.selectedBall.transform.position.y, alphaModifier.selectedBall.transform.position.z,
            headPos.x, headPos.y, headPos.z,
            eyeDir.x, eyeDir.y, eyeDir.z
            );
        }
        else
        {
            client.Send (address, 
            alphaModifier.center.x, alphaModifier.center.y, alphaModifier.center.z,
            -1, -1, -1,
            headPos.x, headPos.y, headPos.z,
            eyeDir.x, eyeDir.y, eyeDir.z
            );
        }
    }
}