using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownUI : MonoBehaviour
{
    public TMP_Text _Text;

    public void StartCountDown(int howlong,int wave)
    {
        StopAllCoroutines();
        StartCoroutine(CountDown(howlong,wave));
    }
    public IEnumerator CountDown(int howlong,int wave)
    {
        int dur = howlong;
        yield return new WaitForSeconds(1);
        dur--;
        //Debug.Log(dur);
        while (dur > 0)
        {
            if (dur <= 5)
            {
                SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_countdown);
            }
            _Text.text = ((int)dur).ToString();
            dur--;
            //Debug.Log(dur);
            yield return new WaitForSeconds(1);
        }
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_start);
        _Text.text = "第"+wave+"波";
        yield return new WaitForSeconds(2);
        _Text.text = "";
    }
}
