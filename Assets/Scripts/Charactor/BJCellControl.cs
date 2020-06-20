using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJCellControl : CellBase
{
    //bool canInit = false;
    public override void InitCell()
    {
        base.InitCell();
        //canInit = false;
    }

    public override void StopAction()
    {

    }
    public override void StartAction()
    {

    }
    public override void OnEnemyEnter(Transform enemyTrans)
    {
        
            ControlManager.Instance.StartCoroutine(ControlManager.Instance.DelayInitOneCell(CellType.JX, LevelManager.Instance.GetNodeByPos(base.gridPos),0.1f));
            //canInit = false;
            Destroy(gameObject);
        
        
    }
    
}
