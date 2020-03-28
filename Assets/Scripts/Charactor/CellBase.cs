using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ShortRangeAttack
{
    void InitEnemysInRange();//更新在攻击范围内的敌人，每当一个敌人进出攻击范围的时候调用
    Transform ChooseTargetEnemey();//选择目标敌人
    void AttackOneTime(Transform enemy, float damage);//攻击一次敌人

   
}
public interface LongRangeAttack
{
    void InitEnemysInRange();
    void AttackOnce(Transform[] enemys, float damage);
}

public interface Produce
{
    void Produce();
}
//所有免疫细胞的基类
public class CellBase : MonoBehaviour
{
    public float InitCost;//部署点数
    public CellStatus cellStatus;//当前状态

   

    public virtual void InitCell() { }

}
