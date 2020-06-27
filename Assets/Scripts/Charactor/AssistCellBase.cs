using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssistCellBase : CellBase,Produce
{
    public Slider ProduceSlider;
    protected float reloadTime;
    protected bool allowProduce;
   
    
    public override void InitCell()
    {
        base.InitCell();
        ProduceSlider = transform.Find("SliderCanvas").Find("PrdSlider").GetComponent<Slider>();

        ProduceSlider.value = 0;
        reloadTime = 0;
        allowProduce = false;
    }
    protected virtual void FixedUpdate()
    {
        if (cellStatus == CellStatus.Die||!allowProduce) { return; }

        ProduceSlider.value = Mathf.Clamp(reloadTime / atkDuration, 0, 1);
        if (reloadTime < atkDuration)
        {
            reloadTime += Time.deltaTime;
            
        }
        else
        {
            Produce();
            reloadTime = 0;
        }
    }

    public virtual void Produce()
    {

    }

    public override void StartAction()
    {
        //Debug.Log("1");
        allowProduce = true;
        cellAnimator.CleanFrameData();
        cellAnimator.reverse = false;
        OnCellStatusChange(CellStatus.Change);
    }

    public override void StopAction()
    {
        //Debug.Log("结束");
        allowProduce = false;
        cellAnimator.CleanFrameData();
        cellAnimator.reverse = true;
        OnCellStatusChange(CellStatus.Change);
    }
}
