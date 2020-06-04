﻿using System.Collections;
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
        InitCell();
        eatTimes = 15;
        attackType = AttackType.Swallow;
        slider.value = 1;
    }

    public override void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if(targetEnemy == null)
        {
            cellStatus = CellStatus.Idle;
            return;
        }
        if (targetEnemy.GetComponent<EnemyHealth>().Hp <= atkDamage* JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType))
        {
            cellStatus = CellStatus.SpecialAbility;
            targetEnemy.GetComponent<EnemyMotion>().TargetPoint = eatTrans.position;
            SetDamageToEnemy(attackType);
            cellAnimator.CleanFrameData();
            eatTimes--;
            slider.value = (float)eatTimes / 15.0f;
            
            if (eatTimes <= 0) EatTooMuch();
        }
        else
        {

            cellStatus = CellStatus.Attack;
            Invoke("SetDamageToEnemy", atkTime);
        }
    }

    public void EatTooMuch()
    {
        cellStatus = CellStatus.Die;
        slider.transform.parent.gameObject.SetActive(false);
    }
    /*public void OnDie()
    {
        Destroy(gameObject);
    }*/

}
    

