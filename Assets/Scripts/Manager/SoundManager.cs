using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;


    private float soundVolumn;



    public AudioClip
        sfx_btnY, sfx_btnN,//按键
        sfx_enemy_dead, sfx_enemy_escape,//敌人死亡，敌人逃离
        sfx_gain_bushu,//部署点数
        sfx_upgrade,//细胞升级
        sfx_bomb,//炸弹
        sfx_fail, sfx_success;//失败，成功

    int frameBuffer = 5;
    bool inBuffer = false;
    [SerializeField] AudioClip sfxInBuffer;

    bool sfxOverride;
    [SerializeField] AudioClip overrideClip;

    private List<AudioSource> audioSourceList;        //Drag a reference to all the audio source which will play sfx in game

    public enum SoundResource
    {
        //高
        sfx_btnY = 0,//确认
        sfx_btnN = 1,//取消
        sfx_enemy_dead = 2,//敌人死亡
        sfx_enemy_escape = 3,//敌人逃离
        sfx_gain_bushu = 4,//部署点数
        sfx_upgrade = 5,//细胞升级
        sfx_bomb = 6,//炸弹
        sfx_fail = 7,//失败
        sfx_success = 8,//成功
        //中
        sfx_enemy_hit = 9,//敌人掉血
        sfx_countdown = 10,//倒计时
        sfx_start = 11,//开始
        sfx_shovel_select = 12,//选中铲子
        sfx_root_out = 13,//铲除
    }

    public void PlaySoundEffect(int soundresource){
        switch (soundresource)
        {
            case 0:
                PlaySingle(sfx_btnY);
                break;
            case 1:
                PlaySingle(sfx_btnN);
                break;
            case 2:
                PlaySingle(sfx_enemy_dead);
                break;
            case 3:
                PlaySingle(sfx_enemy_escape);
                break;
            case 4:
                PlaySingle(sfx_gain_bushu);
                break;
            case 5:
                PlaySingle(sfx_upgrade);
                break;
            case 6:
                PlaySingle(sfx_bomb);
                break;
            case 7:
                PlaySingle(sfx_fail);
                break;
            case 8:
                PlaySingle(sfx_success);
                break;

        }
        }

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
        audioSourceList = new List<AudioSource>();

        sfxOverride = false;
        overrideClip = null;

        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        ValueChangeCheck(soundVolume);

        if (PlayerPrefs.HasKey("SoundVolumn"))
        {
            soundVolumn = PlayerPrefs.GetFloat("SoundVolumn");
            
        }
        else
        {
            soundVolumn = 1;
        }
    }
    public void ValueChangeCheck(float vol)
    {
        //Debug.Log("sound volume changes to:" + vol);
        PlayerPrefs.SetFloat("SoundVolume", vol);
       
        foreach (AudioSource audioSource in audioSourceList)
        {
            audioSource.volume = vol;
        }

    }
    public void AddToSoundList(AudioSource[] newSources)
    {
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        //Debug.Log(soundVolume);
        foreach (AudioSource source in newSources)
        {
            if (audioSourceList.Contains(source))
            {
                continue;
            }
            //Debug.Log("Add sound:" + source.name);
            source.volume = soundVolume;
            audioSourceList.Add(source);
        }

    }
    public void RemoveFromSoundList(AudioSource[] Sources)
    {
        foreach (AudioSource source in Sources)
        {
            if (audioSourceList.Contains(source))
            {
                audioSourceList.Remove(source);
            }
            //Debug.Log("Add sound:" + source.name);

        }

    }
    public void pauseAllTheSounds()
    {
        foreach (AudioSource audioSource in audioSourceList)
        {
            audioSource.Pause();
        }
    }

    public void unPauseAllTheSounds()
    {
        

        foreach (AudioSource audioSource in audioSourceList)
        {
            audioSource.UnPause();
        }
    }
   
    //音效引用
    public void PlayButtonYes()
    {
        PlaySingle(sfx_btnY);
    }
    public void PlayButtonNo()
    {
        PlaySingle(sfx_btnN);
    }
    public void PlayEnemyDead()
    {
        PlaySingle(sfx_enemy_dead);
    }
    public void PlayEnemyEscape()
    {
        PlaySingle(sfx_enemy_escape);
    }
    public void PlayGainBushu()
    {
        PlaySingle(sfx_gain_bushu);
    }
    public void PlayUpgrade()
    {
        PlaySingle(sfx_upgrade);
    }
    public void PlayBomb()
    {
        PlaySingle(sfx_bomb);
    }
    public void PlayFail()
    {
        PlaySingle(sfx_fail);
    }
    public void PlaySuccess()
    {
        PlaySingle(sfx_success);
    }



    //--------------------------//
    //	ALL-PURPOSE FUNCTIONS	//
    //--------------------------//

    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Debug.Log("Audio: PlaySingle " + clip.name);

        //the backup source is a failsafe in case all audio sources are playing.
        //in this case, we want the new clip to override currently playing sound effects

        if (inBuffer && clip == sfxInBuffer)
        {
            //if we're in the buffer window and the clip is assigned as
            //the buffer clip, then it has just been played and doesn't need 
            //to be played again
            // Debug.Log("Audio: Prevented Simultaneous SFX: " + sfxInBuffer.name);

            return;
        }
        else if (sfxOverride && clip != overrideClip)
        {
            Debug.Log("Audio: Overridden SFX: " + clip.name);
            return;
        }
        else
        {
            //otherwise, assign the clip to sfxInBuffer
            sfxInBuffer = clip;
        }

        StartCoroutine("SameSoundCooldown");
    

        }
    public IEnumerator SameSoundCooldown()
    {
        inBuffer = true;

        for (int i = 0; i < frameBuffer; i++)
        {
            yield return null;
        }

        inBuffer = false;
    }


}
