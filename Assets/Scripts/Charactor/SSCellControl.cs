using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSCellControl : LRCellBase
{
    public GameObject bomb;
    public Vector3 bombInitPlaceLeft;
    public Vector3 bombInitPlaceRight;

    public override void AttackOneTime()
    {
        attackType = AttackType.Other;
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
        {
            StopAction();
            OnCellStatusChange(CellStatus.Idle);
            cellAnimator.CleanFrameData();
            return;
        }
        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        Invoke("FireWeapon", 0.35f);

    }
    public override void FireWeapon()
    {
        if (targetEnemy == null)
        {
            OnCellStatusChange(CellStatus.Idle);
            cellAnimator.CleanFrameData();
            return;
        }
        CheckLeftOrRight(targetEnemy);
        Vector3 start = transform.position;
        start += cellAnimator.direction==Direction.Left?
                                                bombInitPlaceLeft:
                                                bombInitPlaceRight;
        Vector3 end = targetEnemy.position;
        //Debug.Log(end);
        Vector3 control = (start + end)/2 + new Vector3(0, 0.5f, 0);
        bomb.SetActive(true);
        bomb.transform.position = start;
        //Debug.Log(targetEnemy.GetComponent<EnemyMotion>().enemyType);
        float coefficient = JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType);
        bomb.GetComponent<BombMotion>().damage = atkDamage*coefficient;
        bomb.GetComponent<BombMotion>().wayPoints =
            MyTool.GetBeizerList(start, control, end, 20);

    }
}
