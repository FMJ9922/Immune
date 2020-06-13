using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JXCellControl : LRCellBase
{
    public GameObject Anti;
    public List<GameObject> enemys = new List<GameObject>();
    // Start is called before the first frame update


    public override void AttackOneTime()
    {
        attackType = AttackType.Other;
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
        {
            OnCellStatusChange(CellStatus.Idle);
            cellAnimator.CleanFrameData();
            return;
        }
        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        Invoke("FireWeapon", 28 * 0.0166667f);

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
        GameObject AntiBullet = Instantiate(Anti, transform);
        AntiBullet.transform.position = start;
        float coefficient = JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType);
        AntiBullet.GetComponent<AntiMotion>().damage = atkDamage * coefficient;
        AntiBullet.GetComponent<AntiMotion>().SetTarget(targetEnemy.transform);

    }
}

