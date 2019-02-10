using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
 
public class SaveWholeRisk : MonoBehaviour {
    public CalculateWholeRisk calculateWholeRisk;
    public AdjustThresholdByHMDRotation adjust;

    private int count;
    private bool isWriting;
    private StreamWriter sw;
 
    // Use this for initialization
    void Start () {
        count = 0;
        isWriting = false;
    }
     
    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.O) && !isWriting)
        {
            sw = new StreamWriter("Assets/StreamingAssets/Videos/wholeRisk"+
            System.DateTime.Now.Day.ToString() +
            System.DateTime.Now.Hour.ToString() +
            System.DateTime.Now.Minute.ToString() +
            ".csv",true);
            isWriting = true;
            Debug.Log("start log");
        }

        if(isWriting){
            string[] str = {count.ToString(), Mathf.Sqrt(calculateWholeRisk.wholeRisk).ToString(), adjust.magnitude.ToString()};
            string strConnected = string.Join(",", str);
            sw.WriteLine(strConnected);
            count++;
        }

        if(Input.GetKeyDown(KeyCode.P) && isWriting)
        {
            sw.Close();
            isWriting = false;
            Debug.Log("end log");
        }
    }
}