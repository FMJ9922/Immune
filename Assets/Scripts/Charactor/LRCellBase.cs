using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LRCellBase :  CellBase,LongRangeAttack
{

    protected FireMode fireMode;
    private float reloadTime;
    private bool allowAttack;
    public Slider AtkSlider;//攻击进度条
    public Detector detector;

    public override void InitCell() 
    {
        base.InitCell();
        detector = transform.Find("Detector").GetComponent<Detector>();
        detector.GetComponent<CircleCollider2D>().radius = atkRange;
       fireMode = FireMode.Nearest;
        AtkSlider = transform.Find("Canvas").Find("AtkSlider").GetComponent<Slider>();
        AtkSlider.value = 1;
        reloadTime = atkDuration;
        allowAttack = false;
    }
    void FixedUpdate()
    {
        if ( cellStatus == CellStatus.Die) { return; }

        AtkSlider.value = Mathf.Clamp(reloadTime / atkDuration, 0, 1);
        if (reloadTime < atkDuration)
        {
            reloadTime += Time.deltaTime;
        }
        else if(allowAttack)
        {
            AttackOneTime();
            reloadTime = 0;
        }

    }

    public override void StartAction()
    {
        allowAttack = true;
    }

    public override void StopAction()
    {
        allowAttack = false;
    }
    public Transform ChooseTargetEnemey()
    {
        return detector.CheckEnemyArrayList(fireMode, attackType);
    }
    
    public virtual void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
        {
            allowAttack = false;
            OnCellStatusChange(CellStatus.Idle);
            cellAnimator.CleanFrameData();
            return;
        }
        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        Invoke("FireWeapon", atkTime);

    }
    public virtual void FireWeapon()
    {

    }
   
}
