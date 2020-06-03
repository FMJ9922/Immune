using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfoUI : MonoBehaviour
{
    public static LevelInfoUI Instance { get; private set; } = null;
    void Awake()
    {
        if (Instance != null && Instance != this)//检测Instance是否存在且只有一个
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        LevelManager.OnScoreChange += InvalidateInfo;
    }

    private TMP_Text content0;
    private TMP_Text content1;
    private TMP_Text content2;
    void Start()
    {
        content0 = transform.Find("request0").GetComponent<TMP_Text>();
        content1 = transform.Find("request1").GetComponent<TMP_Text>();
        content2 = transform.Find("request2").GetComponent<TMP_Text>();
        InvalidateInfo(JsonIO.GetScoreRequest());
    }




    public void InvalidateInfo(ScoreRequest[] scoreRequests)
    {
        content0.text = MyTool.PraseRequest(scoreRequests[0].scoreType, scoreRequests[0].requestNum, scoreRequests[0].actualNum);
        content1.text = MyTool.PraseRequest(scoreRequests[1].scoreType, scoreRequests[1].requestNum, scoreRequests[1].actualNum);
        content2.text = MyTool.PraseRequest(scoreRequests[2].scoreType, scoreRequests[2].requestNum, scoreRequests[2].actualNum);

    }
}
