using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoadButton : MonoBehaviour
{
    delegate void ButtonName(int level);
    void Start()
    {
        ButtonName OnClick = new ButtonName(LoadGameLevel);
        OnClick += LoadGameLevel;
        for (int i = 0; i < gameObject.GetComponentsInChildren<Button>().Length; i++)
        {
            Button btn = gameObject.GetComponentsInChildren<Button>()[i];
            btn.onClick.RemoveAllListeners();
            
            btn.onClick.AddListener(() =>{ OnClick(int.Parse(btn.name));
                SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_btnY);
            });
        }
    }

    public void LoadGameLevel(int level)
    {
        GameManager.Instance.LoadLevelScene(level);
    }
}

