using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SZCellControl : SRCellBase
{
    public Transform eatTrans;
    private int eatTimes;
    private void Start()
    {
        InitCell();
        eatTimes = 10;
        attackType = AttackType.Swallow;
    }

    public override void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if(targetEnemy == null)
        {
            cellStatus = CellStatus.Idle;
            return;
        }
        if (targetEnemy.GetComponent<EnemyHealth>().Hp <= atkDamage)
        {
            cellStatus = CellStatus.SpecialAbility;
            targetEnemy.GetComponent<EnemyMotion>().TargetPoint = eatTrans.position;
            SetDamageToEnemy(attackType);
            cellAnimator.CleanFrameData();
            eatTimes--;
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
    }
    /*public void OnDie()
    {
        Destroy(gameObject);
    }*/

}
    

