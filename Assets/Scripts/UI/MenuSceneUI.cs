using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneUI : MonoBehaviour
{
    public Canvas[] menuCanvas;
    public InputField playerName;
    private void Start()
    {
        //playerName = transform.Find("SettingCanvas").Find("Layout").Find("InputPlayerName").GetComponent<InputField>();
        //Debug.Log(playerName.transform.name);
        //playerName.text = PlayerPrefs.HasKey("PlayerName") ? PlayerPrefs.GetString("PlayerName") : "";
    }
    public void CloseAllCanvas()
    {
        foreach (Canvas canvas in menuCanvas)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    
    public void OpenGameSelectCanvas()
    {
        CloseAllCanvas();
        menuCanvas[0].gameObject.SetActive(true);
    }
    public void OpenIllustrationCanvas()
    {
        CloseAllCanvas();
        menuCanvas[1].gameObject.SetActive(true);
    }
    public void OpenSettingCanvas()
    {
        CloseAllCanvas();
        menuCanvas[2].gameObject.SetActive(true);
    }
    public void OpenAboutCanvas()
    {
        CloseAllCanvas();
        menuCanvas[3].gameObject.SetActive(true);
    }

    public void ApplySettingData()
    {
        PlayerPrefs.SetFloat("MusicVolumn", MusicManager.Instance.Volumn);
        PlayerPrefs.SetFloat("SoundVolumn", SoundManager.Instance.Volumn);
        CloseAllCanvas();
    }
}
