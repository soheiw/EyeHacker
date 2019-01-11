using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    public Camera targetCamera;

	// Use this for initialization
	void Start () {
		if(targetCamera == null)
        {
            targetCamera = Camera.main;
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(targetCamera.transform.position);
	}
}
