using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    private float soundVolumn;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }
    public float Volumn
    {
        get
        {
            return soundVolumn;
        }
        set
        {
            soundVolumn = Mathf.Clamp(value,0,1);
        }
    }
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

        if (PlayerPrefs.HasKey("SoundVolumn"))
        {
            soundVolumn = PlayerPrefs.GetFloat("SoundVolumn");
        }
        else
        {
            soundVolumn = 1;
        }
    }
}
