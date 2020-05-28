using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneUI : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void ApplySettingData()
    {
        PlayerPrefs.SetFloat("MusicVolumn", MusicManager.Instance.Volumn);
        PlayerPrefs.SetFloat("SoundVolumn", SoundManager.Instance.Volumn);
    }

    public void RestartLevel()
    {
        GameManager.Instance.LoadLevelScene(GameManager.Instance.iLevel);
    }

    public void LoadNextLevel()
    {
        GameManager.Instance.LoadLevelScene(GameManager.Instance.iLevel+1);
    }
    public void OpenMenuScene()
    {
        GameManager.Instance.LoadMenuScene();

    }
}
