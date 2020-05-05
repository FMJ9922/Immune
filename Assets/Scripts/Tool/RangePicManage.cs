using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangePicManage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool shineLoop;
    private IEnumerator enumerator;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        shineLoop = false;
        //ControlManager.OnClickTile += ClosePic;
    }
    /*public void ClosePic(bool close = true)
    {
        if (close)
        {
            if(enumerator!=null)
                StopCoroutine(enumerator);
            Debug.Log("Close");
            ChangeLocalScale(0);
        }
    }*/
    public void ChangeLocalScale(float targetScale)
    {
        //Debug.Log("?");
        if (enumerator != null)
            StopCoroutine(enumerator);
        enumerator = ChangeScale(targetScale*0.6f);
        StartCoroutine(enumerator);
        shineLoop = !shineLoop;
    }
    private IEnumerator ChangeScale(float targetScale)
    {
        Vector3 local = transform.localScale;
        float time = 0;
        while ( !Mathf.Approximately(targetScale, transform.localScale.x))
        {
            //Debug.Log(transform.localScale.x);
            transform.localScale = (targetScale * Vector3.one - local) * (1 - Mathf.Pow(0.5f, time)) + local;
            time += 3*Time.deltaTime;
            yield return 0;
        }
    }
    private void FixedUpdate()
    {
        if(shineLoop)
            DoShineLoop(0.8f, 0.2f, 5f);
    }

    private void DoShineLoop(float center,float rad,float speed)
    {
        float alpha = rad*Mathf.Cos(Time.time*speed)+center;
        //Debug.Log(alpha);
        spriteRenderer.color = new Color(spriteRenderer.color.r,
                                spriteRenderer.color.g,
                                spriteRenderer.color.b,
                                alpha);
    }
}
