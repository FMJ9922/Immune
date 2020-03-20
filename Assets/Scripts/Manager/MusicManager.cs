using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;
    private float musicVolumn;
    public static MusicManager Instance
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
            return musicVolumn;
        }
        set
        {
            musicVolumn = Mathf.Clamp(value, 0, 1);
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

        if (PlayerPrefs.HasKey("MusicVolumn"))
        {
            musicVolumn = PlayerPrefs.GetFloat("MusicVolumn");
        }
        else
        {
            musicVolumn = 1;
        }
    }
}
