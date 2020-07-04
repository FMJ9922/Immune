using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NKCellControl : SRCellBase
{
    // Start is called before the first frame update
    void Start()
    {
        attackType = AttackType.Other;
        Debug.Log(atkDamage);
    }

    public override void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
        {
            OnCellStatusChange(CellStatus.Idle);
            allowAttack = false;
            return;
        }

        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        Invoke("SetDamageToEnemy", atkTime);
        
    }
}
