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
    private static EyeData eyeData;
    private static VerboseData verboseData;

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
        Vector3 cameraRot = Camera.main.transform.rotation.eulerAngles;

        SRanipal_Eye.GetEyeData (ref eyeData);
        SRanipal_Eye.GetVerboseData (out verboseData);

        Vector3 eyeDir = eyeData.verbose_data.combined.eye_data.gaze_direction_normalized;

        if (alphaModifier.selectedBall != null)
        {
            client.Send (address, alphaModifier.center.x, alphaModifier.center.y, alphaModifier.center.z, alphaModifier.selectedBall.transform.position.x, alphaModifier.selectedBall.transform.position.y, alphaModifier.selectedBall.transform.position.z, cameraRot.x, cameraRot.y, cameraRot.z, eyeDir.x, eyeDir.y, eyeDir.z);
        }
        else
        {
            client.Send (address, alphaModifier.center.x, alphaModifier.center.y, alphaModifier.center.z, -1, -1, -1, cameraRot.x, cameraRot.y, cameraRot.z, eyeDir.x, eyeDir.y, eyeDir.z);
        }
    }
}