using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRCellBase : CellBase, ShortRangeAttack
{
    protected FireMode fireMode;
    protected  float reloadTime;
    protected bool allowAttack;
    protected Slider AtkSlider;//攻击进度条
    protected  Detector detector;

    public override void InitCell()
    {
        base.InitCell();
        detector = transform.Find("Detector").GetComponent<Detector>();
        detector.GetComponent<CircleCollider2D>().radius = atkRange;
        fireMode = FireMode.First;
        AtkSlider = transform.Find("Canvas").Find("AtkSlider").GetComponent<Slider>();
        AtkSlider.value = 1;
        reloadTime = atkDuration;
        allowAttack = false;
        attackType = AttackType.Other;
    }
    
    protected virtual void FixedUpdate()
    {
        if ( cellStatus == CellStatus.Die) { return; }

        AtkSlider.value = Mathf.Clamp(reloadTime/ atkDuration,0,1);
        if (reloadTime <atkDuration)
        {
            reloadTime += Time.deltaTime;
        }
        else if (allowAttack)
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
            OnCellStatusChange(CellStatus.Idle);
            cellAnimator.CleanFrameData();
            return;
        }
        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        Invoke("SetDamageToEnemy", atkTime);
    }
    public void SetDamageToEnemy()
    {
        if (targetEnemy != null)
        {
            float coefficient = JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType);
            targetEnemy.GetComponent<EnemyHealth>().TakeDamage(atkDamage* coefficient, false);
        }
    }
    public void SetDamageToEnemy(AttackType attackType)
    {
        if (targetEnemy != null&&attackType==AttackType.Swallow)
        {
            Debug.Log("eat");
            float coefficient = JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType);
            targetEnemy.GetComponent<EnemyHealth>().TakeDamage(atkDamage* coefficient, true);
        }
    }
}
