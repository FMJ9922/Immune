using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;
    public string playerName;
    private FadeScene fadeScene;
    public int iLevel //当前关卡，如果不是关卡值为0，否则为关卡序号，从1开始
    {
        set
        {
            currentLevel = value;
            //Debug.Log("set level to:" + currentLevel);
            if (value != 0) JsonIO.InitLevelData(iLevel);

        }
        get
        {
            return currentLevel;
        }
    }
    [SerializeField] private int currentLevel = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)//检测Instance是否存在且只有一个
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            Debug.Log("Change Instance");
        }

        DontDestroyOnLoad(this.gameObject);//加载关卡时不销毁GameManager

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            playerName = "";
        }
        JsonIO.InitGameData();//加载游戏数据
        JsonIO.InitCellData();//加载细胞数据
        //Debug.Log(iLevel + " " + currentLevel);
        if(iLevel!=0)
            JsonIO.InitLevelData(currentLevel);
        

    }

    //用于加载除游戏关卡外的场景
    private void LoadScene(string sceneName)
    {
        FindFadeImage();
        fadeScene.Fade(1f, 0.5f);
        ClearLevelData();
        iLevel = 0;
        StartCoroutine(DelayToInvokeDo(() => { SceneManager.LoadScene(sceneName, LoadSceneMode.Single); }, 1f));
    }

    //重载，用于加载游戏关卡场景
    private void LoadScene(int levelNum)
    {
        FindFadeImage();
        fadeScene.Fade(1f, 0.5f);
        ClearLevelData();
        iLevel = levelNum;
        StartCoroutine(DelayToInvokeDo(() => { SceneManager.LoadScene("LoadScene", LoadSceneMode.Single); }, 1f));
        //JsonIO.InitLevelData(ilevel);//更新关卡数据
    }

    //清空关卡数据，在加载关卡之前
    private void ClearLevelData()
    {

    }
    public void LoadMenuScene()
    {
        
        LoadScene("MenuScene");
    }

    public void LoadLevelScene(int levelNum)
    {
        LoadScene(levelNum);
    }

    //退出游戏程序
    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void FindFadeImage()
    {
        if (GameObject.Find("FadeImage"))
        {
            fadeScene = GameObject.Find("FadeImage").GetComponent<FadeScene>();
        }
        else
        {
            fadeScene = null;
            Debug.Log("There is no GameObject names 'FadeImage' in this scene");
        }
    }

    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    public  void Set2xTimeScale()//二倍速
    {
        Time.timeScale = 2.0f;
    }
    public  void Set1xTimeScale()//一倍速
    {
        Time.timeScale = 1.0f;
    }
    public  void Set0xTimeScale()//暂停
    {
        Time.timeScale = 0.0f;
    }

}
