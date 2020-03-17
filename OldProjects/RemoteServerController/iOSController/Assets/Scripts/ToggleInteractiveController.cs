using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInteractiveController : MonoBehaviour
{
    public Toggle[] toggles;

    bool isActive;

    // Use this for initialization
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        isActive = true;

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                isActive = false;
                break;
            }
        }

        this.GetComponent<Toggle> ().interactable = isActive;
    }
}