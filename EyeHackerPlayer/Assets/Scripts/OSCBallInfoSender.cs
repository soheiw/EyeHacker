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
    [SerializeField] private

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
        // Vector3 cameraPos = Quaternion.Euler(9.0f * transform.forward) * cameraRot;

        if (alphaModifier.selectedBall != null)
        {
            Vector3 cameraRot = Camera.main.transform.rotation.eulerAngles;
            client.Send (address, alphaModifier.center.x, alphaModifier.center.y, alphaModifier.center.z, alphaModifier.selectedBall.transform.position.x, alphaModifier.selectedBall.transform.position.y, alphaModifier.selectedBall.transform.position.z, cameraRot.x, cameraRot.y, cameraRot.z);
        }
        else
        {
            Vector3 cameraRot = Camera.main.transform.rotation.eulerAngles;
            client.Send (address, alphaModifier.center.x, alphaModifier.center.y, alphaModifier.center.z, -1, -1, -1, cameraRot.x, cameraRot.y, cameraRot.z);
        }
    }
}