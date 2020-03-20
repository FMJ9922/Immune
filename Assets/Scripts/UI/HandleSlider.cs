using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSlider : MonoBehaviour
{
    private Slider slider;
    private Text text;
    private void Start()
    {
        slider = transform.GetComponent<Slider>();
        text = transform.Find("TextNum").GetComponent<Text>();
        if (transform.name == "MusicVolumn")
        {
            slider.value = MusicManager.Instance.Volumn * 100 ;
        }
        if (transform.name == "SoundVolumn")
        {
            slider.value = SoundManager.Instance.Volumn * 100;
        }
    }
    public void OnSliderValueChange()
    {
        text.text = slider.value.ToString();
        if(transform.name == "MusicVolumn")
        {
            MusicManager.Instance.Volumn = (float)slider.value / 100;
        }
        if (transform.name == "SoundVolumn")
        {
            SoundManager.Instance.Volumn = (float)slider.value / 100;
        }
    }
}
