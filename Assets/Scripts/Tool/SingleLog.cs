using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SingleLog : MonoBehaviour
{
    public TMP_Text logText;
    public TMP_Text counterText;
    private float counter = 5;
    Color color;

    public void SetLogInfo(string tMP_Text,int n)
    {
        logText.text = tMP_Text;
        color = logText.color;
        counterText.text = n == 0 ? "" : n.ToString()+ "*";
            
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
            if (counter > 2)
            {

                logText.color = new Color(color.r, color.g, color.b, counter / 2);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
