using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBgUI : MonoBehaviour
{
    public static PauseBgUI Instance { get; private set; }
    void Awake()
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
    public void ShowWhiteBg()
    {
        transform.GetChild(0).gameObject.SetActive(true);    
    }

    public void HideWhiteBg()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
