using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRCellBase : CellBase,ShortRangeAttack
{
    private float atkRange;//攻击范围
    private float atkDamage;//攻击伤害
    private float atkDuration;//冷却时间
    private float atkTime;//攻击时长
    private bool isAttack;//是否正在攻击
    public Transform[] enemyInRange;//范围内的敌人
    public Transform targetEnemy;//目标敌人

    public override void InitCell() { }
    public void InitEnemysInRange()
    {
        
    }

    public Transform ChooseTargetEnemey()
    {
        return null;
    }

    public void AttackOneTime(Transform enemy, float damage)
    {
        
    }
}
