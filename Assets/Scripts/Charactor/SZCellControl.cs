using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SZCellControl : SRCellBase
{
    public Transform eatTrans;
    private void Start()
    {
        InitCell();
        //InvokeRepeating("AttackOneTime", 0, atkDuration);
        //AttackOneTime();
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
            SetDamageToEnemy(EnemyStatus.Engulfed);
            cellAnimator.CleanFrameData();
            
        }
        else
        {

            cellStatus = CellStatus.Attack;
            Invoke("SetDamageToEnemy", atkTime);
        }
    }

}
    

