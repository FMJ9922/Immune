using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownUI : MonoBehaviour
{
    public TMP_Text _Text;

    public void StartCountDown(int howlong)
    {
        StopAllCoroutines();
        StartCoroutine(CountDown(howlong));
    }
    public IEnumerator CountDown(int howlong)
    {
        int dur = howlong;
        yield return new WaitForSeconds(1);
        dur--;
        //Debug.Log(dur);
        while (dur > 0)
        {
            _Text.text = ((int)dur).ToString();
            dur--;
            //Debug.Log(dur);
            yield return new WaitForSeconds(1);
        }
        _Text.text = "Go!";
        yield return new WaitForSeconds(1);
        _Text.text = "";
    }
}
