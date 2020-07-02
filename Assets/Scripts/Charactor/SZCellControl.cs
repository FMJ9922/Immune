using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SZCellControl : SRCellBase
{
    public Transform eatTrans;
    private int eatTimes;
    public Slider slider;
    private void Start()
    {
        eatTimes = 15;
        attackType = AttackType.Swallow;
        slider.value = 1;
    }
   
    public override void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if(targetEnemy == null)
        {
            OnCellStatusChange(CellStatus.Idle);
            return;
        }
        EnemyMotion enemyMotion = targetEnemy.GetComponent<EnemyMotion>();
        if (targetEnemy.GetComponent<EnemyHealth>().Hp <= 
            atkDamage* JsonIO.GetCoefficiet(cellType, enemyMotion.enemyType)
            * enemyMotion.GetCoefficient())
        {
            OnCellStatusChange(CellStatus.SpecialAbility);
            targetEnemy.GetComponent<EnemyMotion>().TargetPoint = eatTrans.position;
            SetDamageToEnemy(attackType);
            cellAnimator.CleanFrameData();
            eatTimes--;
            slider.value = (float)eatTimes / 15.0f;
            
            if (eatTimes <= 0) EatTooMuch();
        }
        else
        {
            cellAnimator.CleanFrameData();
            OnCellStatusChange(CellStatus.Attack);
            Invoke("SetDamageToEnemy", atkTime);
        }
    }

    public void EatTooMuch()
    {
        OnCellStatusChange(CellStatus.Die);
        slider.transform.parent.gameObject.SetActive(false);
    }
    public override void OnEnemyEnter(Transform enemyTrans)
    {
        base.OnEnemyEnter(enemyTrans);
        EnemyStatus status = enemyTrans.GetComponent<EnemyMotion>().enemyStatus;
        if (status == EnemyStatus.Engulfed)
        {
            return;
        }
        else
        {
            StartAction();
        }

    }

}
    

