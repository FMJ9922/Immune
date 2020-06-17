using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TYCellControl : AssistCellBase
{
    public Button OnOffBtn;
    public TMP_Text text;
    public bool isAllow;
    public delegate void OnTYTakingActionDelegate(bool isAllow);
    public event OnTYTakingActionDelegate OnTYTakingAction; 
    public override void InitCell()
    {
        base.InitCell();
        transform.GetComponent<CircleCollider2D>().radius = atkRange;
        allowProduce = true;
        isAllow = true;
        cellAnimator.direction = Direction.Right;
        
    }

    new void OnDestroy()
    {
        cellAnimator.OnStatusChange -= OnCellStatusChange;
        StopAllCoroutines();
        OnTYTakingAction = null;
    }
    public override void StartAction()
    {
        if (OnTYTakingAction != null)
        {
            OnTYTakingAction(false);
        }
        cellAnimator.CleanFrameData();
        cellAnimator.reverse = false;
        OnCellStatusChange(CellStatus.Change);

    }

    public override void StopAction()
    {
        if (OnTYTakingAction != null)
        {
            OnTYTakingAction(true);
        }
        cellAnimator.CleanFrameData();
        cellAnimator.reverse = true;
        OnCellStatusChange(CellStatus.Change);
    }
    public override void ShowRangePic()
    {
        base.ShowRangePic();
        OnOffBtn.gameObject.SetActive(true);
    }
    public override void CloseRangePic()
    {
        base.CloseRangePic();
        OnOffBtn.gameObject.SetActive(false);
    }

    public void SwitchButton()
    {
        if (isAllow)
        {
            text.text = "抑制：开启";
            isAllow = false;
            StartAction();
        }
        else
        {
            text.text = "抑制：关闭";
            isAllow = true;
            StopAction();
        }
    }
}
