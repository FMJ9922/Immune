using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FireModeHandle : MonoBehaviour
{
    private Button leftBtn;
    private Button rightBtn;
    public FireMode fireMode;
    private TMP_Text text;
    public delegate void FireModeChange(FireMode fireMode);
    public event FireModeChange OnFireModeChange;

    // Start is called before the first frame update
    void Awake()
    {
        leftBtn = transform.Find("LeftBtn").GetComponent<Button>();
        rightBtn = transform.Find("RightBtn").GetComponent<Button>();
        text = transform.Find("Text").GetChild(0).GetComponent<TMP_Text>();
        
    }
    private void OnEnable()
    {
        leftBtn.onClick.AddListener(OnLeftButtonClicked);
        rightBtn.onClick.AddListener(OnRightButtonClicked);
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_btnY);
    }
    private void OnDisable()
    {
        leftBtn.onClick.RemoveListener(OnLeftButtonClicked);
        rightBtn.onClick.RemoveListener(OnRightButtonClicked);
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_btnN);
    }

    private void OnRightButtonClicked()
    {
        int mode = (int)fireMode;
        mode = mode == 3 ? 0 : mode + 1;
        fireMode = (FireMode)mode;
        OnFireModeChange(fireMode);
        text.text = "目标：" + FireModeToString(fireMode);
    }

    public void OnLeftButtonClicked()
    {
        int mode = (int)fireMode;
        mode = mode == 0 ? 3 : mode - 1;
        fireMode = (FireMode)mode;
        OnFireModeChange(fireMode);
        text.text = "目标：" + FireModeToString(fireMode);

    }

    public static string FireModeToString(FireMode fireMode)
    {

        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_btnY);
        string modeString = null;
        switch (fireMode)
        {
            case FireMode.First:
                modeString = "最先";
                break;
            case FireMode.Nearest:
                modeString = "最近";
                break;
            case FireMode.Strongest:
                modeString = "最强";
                break;
            case FireMode.Weakest:
                modeString = "最弱";
                break;
        }
        return modeString;
    }
}
