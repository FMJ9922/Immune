using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleUI : MonoBehaviour
{
    public Sprite scale1;
    public Sprite scale2;
    public Image image;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { SetTimeScale(); });
    }
    public void SetTimeScale()
    {
        if (Time.timeScale == 1)
        {
            GameManager.Instance.Set2xTimeScale();
            GetComponent<Image>().overrideSprite = scale2;
        }
        else
        {
            GameManager.Instance.Set1xTimeScale();
            GetComponent<Image>().overrideSprite = scale1;
        }
        
    }
}
