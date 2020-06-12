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
    Transform ChooseTargetEnemey();
    void AttackOneTime();
}

public interface Produce
{
    void Produce();
}
//所有免疫细胞的基类
public abstract class CellBase : MonoBehaviour
{
    public float InitCost { get; private set; }//部署点数
    public CellStatus cellStatus;//当前状态
    public CellType cellType;//细胞的类型
    public Vector2Int gridPos;//细胞所部署的位置
    protected RangePicManage rangePicManage;//细胞作用范围
    protected AttackType attackType;//细胞攻击方式
    protected CellAnimator cellAnimator;//细胞动画播放类
    public Vector3 revisePosLeft = new Vector3(-0.12f, 0.4f, 0);//细胞动画朝左的锚点
    public Vector3 revisePosRight = new Vector3(0.22f, 0.4f, 0);//细胞动画朝右的锚点
    protected ArrayList enemyInRange;//范围内的敌人
    protected Transform targetEnemy;//目标敌人
    protected CellData cellData;
    protected float atkRange;//技能范围
    protected float atkDamage;//攻击伤害/生产数量
    protected float atkDuration;//冷却时间
    protected float atkTime;//攻击时长

    protected void Awake()
    {
        InitCell();
    }
    public virtual void InitCell()
    {
        cellData = JsonIO.GetCellData(cellType);
        atkDamage = cellData.atkDamage;
        atkDuration = cellData.atkDuration;
        atkRange = cellData.atkRange;
        atkTime = cellData.atkTime;

        enemyInRange = new ArrayList();
        cellAnimator = transform.GetComponentInChildren<CellAnimator>();
        cellAnimator.transform.localPosition = revisePosLeft;
        rangePicManage = transform.GetComponentInChildren<RangePicManage>();
        cellAnimator.OnStatusChange += OnCellStatusChange;

        cellStatus = CellStatus.Idle;
    }
    public ArrayList GetEnemyList()
    {
        return enemyInRange;
    }
    public void ShowRangePic()
    {
        if (rangePicManage == null)
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
    public void CheckLeftOrRight(Transform targetTransfrom)
    {
        if (targetTransfrom == null) return;
        cellAnimator.direction = transform.position.x > targetTransfrom.position.x ?
                                    Direction.Left :
                                    Direction.Right;
        cellAnimator.transform.localPosition = cellAnimator.direction == Direction.Left ?
                                    revisePosLeft :
                                    revisePosRight;
    }

    public void OnEnemyEnter(Transform enemyTrans)
    {
        if (enemyTrans == null) return;
        enemyInRange.Add(enemyTrans);
        EnemyHealth enemyHealth = enemyTrans.GetComponent<EnemyHealth>();
        enemyHealth.cellInRange.Add(this.transform);
        enemyHealth.OnEnemyDie += OnInRangeEnemyDie;

        if (enemyInRange.Count == 1)
        {
            CheckLeftOrRight(enemyTrans);
            StartAction();
        }

        

    }

    public void OnEnemyExit(Transform enemyTrans)
    {
        if (enemyTrans == null) return;
        enemyInRange.Remove(enemyTrans);
        EnemyHealth enemyHealth = enemyTrans.GetComponent<EnemyHealth>();
        enemyHealth.cellInRange.Remove(this.transform);
        enemyHealth.OnEnemyDie -= OnInRangeEnemyDie;
        if (enemyInRange.Count == 0)
        {
            StopAction();
        }
            
    }
    public void OnInRangeEnemyDie(Transform enemyTrans)
    {
        if (enemyTrans == null) return;
        enemyTrans.GetComponent<EnemyHealth>().OnEnemyDie -= OnInRangeEnemyDie;
        if (enemyInRange.Contains(enemyTrans))
        {
            enemyInRange.Remove(enemyTrans);
        }
        if(enemyInRange.Count == 0)
        {
            StopAction();
        }
    }
    public abstract void StartAction();
    public abstract void StopAction();
    public virtual Transform ChooseTargetEnemy()
    {
        return null;
    }
    public void OnCellStatusChange(CellStatus cellStatus)
    {
        if(this.cellStatus != cellStatus)
        {
            //Debug.Log("ChangeTo" + cellStatus.ToString());
            this.cellStatus = cellStatus;
        }
        
        if (cellStatus == CellStatus.Die)
        {
            Invoke("OnDie", 1f);
        }
    }
    public void OnDestroy()
    {
        cellAnimator.OnStatusChange -= OnCellStatusChange;
        StopAllCoroutines();
    }

}
