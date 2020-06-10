using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerManager : MonoBehaviour
{
    public static LoggerManager Instance { get; private set; }
    public GameObject singleLog;
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
        GameObject mylog = Instantiate(singleLog, transform);
        mylog.GetComponent<SingleLog>().SetLogInfo(log);
    }

}
