using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneUI : MonoBehaviour
{

    void Start()
    {

    }



    public void ApplySettingData()
    {
        PlayerPrefs.SetFloat("MusicVolumn", MusicManager.Instance.Volumn);
        PlayerPrefs.SetFloat("SoundVolumn", SoundManager.Instance.Volumn);
    }

    public void RestartLevel()
    {
        PlayBtnYesSound();
        GameManager.Instance.LoadLevelScene(GameManager.Instance.iLevel);
    }

    public void LoadNextLevel()
    {
        PlayBtnYesSound();
        GameManager.Instance.LoadLevelScene(GameManager.Instance.iLevel + 1);
    }
    public void OpenMenuScene()
    {
        PlayBtnYesSound();
        GameManager.Instance.LoadMenuScene();

    }
    public void PlayBtnYesSound()
    {
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_btnY);
    }
    public void PlayBtnNoSound()
    {
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_btnN);
    }
    public void PlayPageTurnSound()
    {
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_page_turn);
    }
    public void PlayShovelSelect()
    {
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_shovel_select);
    }
}
