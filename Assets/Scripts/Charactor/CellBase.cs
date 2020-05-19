using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ShortRangeAttack
{
    //void UpdateEnemysInRange();//更新在攻击范围内的敌人，每当一个敌人进出攻击范围的时候调用
    Transform ChooseTargetEnemey();//选择目标敌人
    void AttackOneTime();//攻击一次敌人

   
}
public interface LongRangeAttack
{
    Transform ChooseTargetEnemeys();
    void AttackOneTime();
}

public interface Produce
{
    void Produce();
}
//所有免疫细胞的基类
public class CellBase : MonoBehaviour
{
    public float InitCost { get; private set; }//部署点数
    public CellStatus cellStatus;//当前状态
    public CellType cellType;
    public Vector2Int gridPos;
    protected RangePicManage rangePicManage;


    public virtual void InitCell() {  }

    public void ShowRangePic()
    {
        if(rangePicManage == null)
        {
            rangePicManage = transform.GetComponentInChildren<RangePicManage>();
        }
        rangePicManage.ChangeLocalScale(JsonIO.GetCellData(cellType).atkRange);
    }
    public void CloseRangePic()
    {
        rangePicManage.ChangeLocalScale(0);
    }

    public void OnDie()
    {
        LevelManager.Instance.GetNodeByPos(transform.position).tileType = TileType.Empty;
        Destroy(gameObject);
    }

}
