using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerManager : MonoBehaviour
{
    public static LoggerManager Instance { get; private set; }
    public GameObject singleLog;
    private SingleLog lastLog;
    private int n = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void ShowOneLog(string log)
    {
        string buildLog = log;
        if (lastLog!=null&&string.Equals(lastLog.logText.text, log))
        {
            n++;
            lastLog.SetLogInfo(log, n);
            return;
        }
        else
        {
            n = 0;
            GameObject mylog = Instantiate(singleLog, transform);
            mylog.GetComponent<SingleLog>().SetLogInfo(buildLog,n);
            lastLog = mylog.GetComponent<SingleLog>();
        }
        
    }

}
