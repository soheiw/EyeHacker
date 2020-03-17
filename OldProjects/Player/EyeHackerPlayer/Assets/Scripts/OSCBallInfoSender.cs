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

        Vector3 center = alphaModifier.center;
        Vector3 headPos = dataWrite.headPosition;
        Vector3 eyeDir = dataWrite.eyeDir;

        if (alphaModifier.selectedBall != null)
        {
            Vector3 ballPos = alphaModifier.selectedBall.transform.position;
            float angle = calculateAngle (Vector3.Distance (center, ballPos));

            client.Send (address,
                center.x, center.y, center.z,
                ballPos.x, ballPos.y, ballPos.z,
                headPos.x, headPos.y, headPos.z,
                eyeDir.x, eyeDir.y, eyeDir.z,
                angle
            );
        }
        else
        {
            client.Send (address,
                center.x, center.y, center.z,
                -1.0f, -1.0f, -1.0f,
                headPos.x, headPos.y, headPos.z,
                eyeDir.x, eyeDir.y, eyeDir.z,
                -1.0f
            );
        }
    }

    float calculateAngle (float d)
    {
        float radius = 9.0f;
        return Mathf.Acos (1.0f - ((d * d) / (2.0f * radius * radius))) * Mathf.Rad2Deg;
    }
}