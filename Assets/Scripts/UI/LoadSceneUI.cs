using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadSceneUI : MonoBehaviour
{
    private Image loadImage;//进度条
    private float progressValue = 0;//加载进度
    private AsyncOperation async = null;//储存异步加载的返回值
    private FadeScene fadeScene;
    public TMP_Text progressText;
    //bool hasLoad = false;
    void Start()
    {
        loadImage = transform.Find("Image").GetComponent<Image>();
        //loadImage.fillAmount = progressValue;
        StartCoroutine("LoadScene");
        fadeScene = transform.Find("FadeImage").GetComponent<FadeScene>();
        progressText = transform.Find("Image").Find("LoadText").GetComponent<TMP_Text>();
    }

    IEnumerator LoadScene()//异步加载场景
    {
        async = SceneManager.LoadSceneAsync("LevelScene");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            /*if (async.progress < 0.9f)
                progressValue = async.progress;
            else
                progressValue = 1.0f;*/

            progressValue = Mathf.Min(async.progress * 10 / 9, progressValue + 0.01f);
            loadImage.fillAmount = progressValue;
            progressText.text = "加载中…"+(int)(loadImage.fillAmount * 100) + " %";

            if (progressValue >= 1)
            {
                progressText.text = "加载完成！请按任意键继续";
                if (Input.anyKeyDown) 
                {
                    //hasLoad = true;
                    fadeScene.Fade(1, 0.5f);
                    Invoke("ActiveLoadLevelScene",0.5f);
                }
            }

            yield return null;
        }

    }
    private void ActiveLoadLevelScene()
    {
        async.allowSceneActivation = true;
    }
}
