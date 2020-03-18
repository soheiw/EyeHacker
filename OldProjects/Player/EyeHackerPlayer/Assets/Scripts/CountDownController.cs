using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

    }

    public void CountDown ()
    {
        StartCoroutine (Count ());
    }

    IEnumerator Count ()
    {
        for (int i = 2; i > 0; i--)
        {
            this.GetComponent<Text> ().text = i.ToString ();
            yield return new WaitForSeconds (1.0f);
        }
    }
}