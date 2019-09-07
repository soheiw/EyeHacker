using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    void Start ()
    {
        Vector3 p = Camera.main.transform.position;
        Vector3 pos = transform.position;
        transform.LookAt (new Vector3 (0.0f, 0.0f, 0.0f));
        pos += 0.25f * transform.forward;
        transform.position = pos;
    }
}