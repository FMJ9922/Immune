﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int ilevel;//当前关卡，如果不是关卡值为0，否则为关卡序号，从1开始
    public string playerName;

    private void Awake()
    {
        if (instance != null && instance != this)//检测Instance是否存在且只有一个
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
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
    }

    //用于加载除游戏关卡外的场景
    private void LoadScene(string sceneName)
    {
        ClearLevelData();
        ilevel = 0;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

    }

    //重载，用于加载游戏关卡场景
    private void LoadScene(int levelNum)
    {
        ClearLevelData();
        ilevel = levelNum;
        SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
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
}
