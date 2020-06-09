using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShovelManager : MonoBehaviour
{
    public Button shovelButton;
    public static ShovelManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        shovelButton = GetComponent<Button>();
        shovelButton.onClick.AddListener(() => {
            OnButtonClick();
        });
    }
    private void OnButtonClick()
    {
        shovelButton.onClick.RemoveAllListeners();
        ControlManager.Instance.OnRemoveButtonDown();
        shovelButton.interactable = false;
    }

    public void OnCancelClose()
    {
        shovelButton.onClick.AddListener(() => {
            OnButtonClick();
        });
        shovelButton.interactable = true;
    }
}
