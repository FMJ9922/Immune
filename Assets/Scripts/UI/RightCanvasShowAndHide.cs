using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightCanvasShowAndHide : MonoBehaviour
{
    public Button button;
    public Animation animation;
    public bool regular;//屏幕是16:9吗
    public static RightCanvasShowAndHide Instance;
    private bool Show;
    public Sprite shows;
    public Sprite hidss;


    private void Start()
    {
        Instance = this;
        regular = ((float)Screen.width / (float)Screen.height) < (17f / 9f) ? true : false;
        if (!regular)
        {
            button.gameObject.SetActive(false);
        }
        else
        {
            animation = GetComponent<Animation>();
            Show = true;
        }
    }

    public void ShowHide()
    {
        if (Show)
        {
            HideCanvas();
        }
        else
        {
            ShowCanvas();
        }

    }
    public void ShowCanvas()
    {
        Show = true;
        animation.Play("RightCanvasShow");
        button.GetComponent<Image>().overrideSprite = shows;

    }
    public void HideCanvas()
    {
        Show = false;
        animation.Play("RightCanvasHide");
        button.GetComponent<Image>().overrideSprite = hidss;


    }
}
