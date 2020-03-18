using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Transform>().position = Camera.main.GetComponent<Transform>().position;
        GetComponent<Transform>().rotation = Camera.main.GetComponent<Transform>().rotation;
	}
}
