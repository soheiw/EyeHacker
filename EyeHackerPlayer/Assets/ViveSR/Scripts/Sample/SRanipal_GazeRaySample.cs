//========= Copyright 2018, HTC Corporation. All rights reserved. ===========
using UnityEngine;
using UnityEngine.Assertions;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class SRanipal_GazeRaySample : MonoBehaviour
            {
                // public Vector2 textureSize = new Vector2 (3008.0f, 1504.0f);
                public Vector3 gazePosition;
                public Vector2 gazeCoord;

                public int LengthOfRay = 25;
                [SerializeField] private LineRenderer GazeRayRenderer;

                private void Start ()
                {
                    if (!SRanipal_Eye_Framework.Instance.EnableEye)
                    {
                        enabled = false;
                        return;
                    }
                    Assert.IsNotNull (GazeRayRenderer);
                }

                private void Update ()
                {
                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;
                    Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;
                    if (SRanipal_Eye.GetGazeRay (GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                    else if (SRanipal_Eye.GetGazeRay (GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                    else if (SRanipal_Eye.GetGazeRay (GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                    else return;

                    Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection (GazeDirectionCombinedLocal);

                    RaycastHit hit;
                    if (Physics.Raycast (Camera.main.transform.position, GazeDirectionCombined, out hit))
                    {
                        gazePosition = transform.TransformPoint (hit.point);
                        gazeCoord = hit.textureCoord;
                        // gazeCoord.x *= textureSize.x;
                        // gazeCoord.y *= textureSize.y;
                        // Debug.Log ("gazeCoord: x: " + gazeCoord.x + ", y: " + gazeCoord.y);
                    }
                    else
                    {
                        gazePosition = transform.TransformPoint (Camera.main.transform.position + GazeDirectionCombined * 50.0f);
                        // gazeCoord = new Vector2 (textureSize.x * 0.5f, textureSize.y * 0.5f);
                        gazeCoord = new Vector2 (0.5f, 0.5f);
                        Debug.Log ("raycast failed.");
                    }

                    GazeRayRenderer.SetPosition (0, Camera.main.transform.position - Camera.main.transform.up * 0.05f);
                    GazeRayRenderer.SetPosition (1, Camera.main.transform.position + GazeDirectionCombined * LengthOfRay);
                }
            }
        }
    }
}