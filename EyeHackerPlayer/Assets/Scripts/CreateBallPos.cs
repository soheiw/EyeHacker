using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateBallPos : MonoBehaviour
{
    public List<List<Vector3>> positionDistributions;

    public Transform staticBalls;
    public Transform changedBalls;

    public GameObject ball;

    private const int WIDTH_COUNT = 80;
    private const int HEIGHT_COUNT = 5; // should be odd number
    private const int TRIAL_COUNT = 48 * 3;
    private const float RADIUS = 9.0f;

    // Start is called before the first frame update
    void Awake ()
    {
        // UnityEngine.Random.InitState (42);
        positionDistributions = new List<List<Vector3>> ();

        for (int k = 0; k < TRIAL_COUNT; k++)
        {
            List<Vector3> distribution = new List<Vector3> ();
            for (int j = 0; j < WIDTH_COUNT; j++)
            {
                float phi_origin = (2.0f * Mathf.PI) * j / WIDTH_COUNT;

                for (int l = -(HEIGHT_COUNT - 1) / 2; l < (HEIGHT_COUNT + 1) / 2; l++)
                {
                    float phi = phi_origin +
                        UnityEngine.Random.Range (-(0.25f / WIDTH_COUNT) * (2.0f * Mathf.PI),
                            (0.25f / WIDTH_COUNT) * (2.0f * Mathf.PI)
                        ); // [-1.125 deg, 1.125 deg] perturbation

                    float theta = (Mathf.PI / 40.0f) * l +
                        UnityEngine.Random.Range (-(0.25f / WIDTH_COUNT) * (2.0f * Mathf.PI),
                            (0.25f / WIDTH_COUNT) * (2.0f * Mathf.PI)
                        ); // [-1.125 deg, 1.125 deg] perturbation

                    Vector3 pos = new Vector3 (
                        RADIUS * Mathf.Cos (theta) * Mathf.Cos (phi),
                        RADIUS * Mathf.Sin (theta),
                        RADIUS * Mathf.Cos (theta) * Mathf.Sin (phi)
                    );
                    distribution.Add (pos);
                }
            }
            // distribution = distribution.OrderBy (i => Guid.NewGuid ()).ToList ();
            positionDistributions.Add (distribution);
            // distribution.Clear ();
        }
        //  positionDistributions = positionDistributions.OrderBy (i => Guid.NewGuid ()).ToList ();

    }

    // instantiate balls referring to list made beforehand
    public void SetBallPositions (int number)
    {
        List<Vector3> positions = positionDistributions[number];

        const int AREA_LENGTH = 10; // should be a divisor of HEIGHT_COUNT * WIDTH_COUNT
        const int CHANGING_OBJECT_NUMBER = 6;

        for (int areaCount = 0; areaCount < AREA_LENGTH; areaCount++)
        {
            List<Vector3> positionsInArea = new List<Vector3> ();
            for (int k = 0; k < (HEIGHT_COUNT * WIDTH_COUNT / AREA_LENGTH); k++)
            {
                positionsInArea.Add (positions[k + areaCount * (HEIGHT_COUNT * WIDTH_COUNT / AREA_LENGTH)]);
            }

            positionsInArea = positionsInArea.OrderBy (i => Guid.NewGuid ()).ToList ();

            for (int n = 0; n < CHANGING_OBJECT_NUMBER; n++)
            {
                GameObject g = Instantiate (ball, positionsInArea[n], Quaternion.identity);
                g.transform.parent = staticBalls;
            }

            for (int m = CHANGING_OBJECT_NUMBER; m < (HEIGHT_COUNT * WIDTH_COUNT / AREA_LENGTH); m++)
            {
                GameObject h = Instantiate (ball, positionsInArea[m], Quaternion.identity);
                Color col = h.GetComponent<Renderer> ().material.color;
                h.GetComponent<Renderer> ().material.color = new Vector4 (col.r, col.g, col.b, 0.0f);
                h.tag = "changedBall";
                h.transform.parent = changedBalls;
            }
            positionsInArea.Clear ();
        }
    }

    public void DestroyAll ()
    {
        foreach (Transform n in staticBalls)
        {
            GameObject.Destroy (n.gameObject);
        }

        foreach (Transform m in changedBalls)
        {
            GameObject.Destroy (m.gameObject);
        }
    }
}