using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSCellControl : SRCellBase
{
    public Transform eatTrans;
    private int eatTimes;
    private void Start()
    {
        InitCell();
        //eatTimes = 10;
        attackType = AttackType.Swallow;
    }

    public override void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
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
        }
        else
        {

            cellStatus = CellStatus.Attack;
            Invoke("SetDamageToEnemy", atkTime);
        }
    }
}
