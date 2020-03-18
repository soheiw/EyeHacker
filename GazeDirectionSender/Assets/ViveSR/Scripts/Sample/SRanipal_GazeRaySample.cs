//========= Copyright 2018, HTC Corporation. All rights reserved. ===========
using UnityEngine;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class SRanipal_GazeRaySample : MonoBehaviour
            {
                Vector3 gazeDirection;

                private void Start ()
                {
                    if (!SRanipal_Eye_Framework.Instance.EnableEye)
                    {
                        enabled = false;
                        return;
                    }
                    gazeDirection = new Vector3 (0.0f, 0.0f, 0.0f);
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

                    gazeDirection = GazeDirectionCombinedLocal;
                }

                public Vector3 GetGazeDirection ()
                {
                    return gazeDirection;
                }
            }
        }
    }
}