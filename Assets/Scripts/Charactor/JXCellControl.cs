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
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
        {
            OnCellStatusChange(CellStatus.Idle);
            cellAnimator.CleanFrameData();
            return;
        }
        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        FireWeapon();

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
        Vector3 end = targetEnemy.position;
        //Debug.Log(end);
        Vector3 control = (start + end) / 2 + new Vector3(0, 0.5f, 0);
        Anti.SetActive(true);
        Anti.transform.position = start;
        float coefficient = JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType);
        Anti.GetComponent<AntiMotion>().damage = atkDamage * coefficient;
        Anti.GetComponent<AntiMotion>().SetTarget(targetEnemy.transform);

    }
}

