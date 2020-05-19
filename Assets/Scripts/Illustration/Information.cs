using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Information : ViewBase 
{
    private RectTransform rectInfo;
    private Text text;

    private void Start()
    {
        rectInfo = transform.Find("info").GetComponent<RectTransform>();
        text = rectInfo.GetComponentInChildren<Text>();
    }
    public void SetShowInfo(string info)
    {
        if (text != null)
        {
            text.text = info;
        }
    }
}

