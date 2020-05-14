using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRCellBase : CellBase, ShortRangeAttack
{
    protected float atkRange;//攻击范围
    protected float atkDamage;//攻击伤害
    protected float atkDuration;//冷却时间
    protected float atkTime;//攻击时长
    protected CellData cellData;
    protected ArrayList enemyInRange;//范围内的敌人
    protected Transform targetEnemy;//目标敌人
    protected CellAnimator cellAnimator;
    public Vector3 revisePosLeft = new Vector3(-0.12f, 0.4f, 0);
    public Vector3 revisePosRight = new Vector3(0.22f, 0.4f, 0);
    protected FireMode fireMode;
    protected IEnumerator enumerator;


    public override void InitCell()
    {
        cellData = JsonIO.GetCellData(cellType);
        atkDamage = cellData.atkDamage;
        atkDuration = cellData.atkDuration;
        atkRange = cellData.atkRange;
        atkTime = cellData.atkTime;
        transform.GetComponent<CircleCollider2D>().radius = atkRange;
        enemyInRange = new ArrayList();
        cellAnimator = transform.GetComponentInChildren<CellAnimator>();
        cellAnimator.transform.localPosition = revisePosLeft;
        this.rangePicManage = transform.GetComponentInChildren<RangePicManage>();
        cellAnimator.OnStatusChange += OnCellStatusChange;
        fireMode = FireMode.Nearest;
        enumerator = null;
        cellStatus = CellStatus.Idle;
    }
    private void Start()
    {
        InitCell();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (enemyInRange.Count == 0)
            {
                CheckLeftOrRight(collision.transform);
            }
            enemyInRange.Add(collision.transform);
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.cellInRange.Add(this.transform);
            enemyHealth.OnEnemyDie += OnInRangeEnemyDie;


            if (enumerator == null)
            {
                enumerator = StartAttackEnemy();
                StartCoroutine(enumerator);
            }
        }

    }
    protected void OnInRangeEnemyDie(Transform enemyTrans)
    {
        enemyTrans.GetComponent<EnemyHealth>().OnEnemyDie -= OnInRangeEnemyDie;
        if (enemyInRange.Contains(enemyTrans))
        {
            enemyInRange.Remove(enemyTrans);
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy")){
            enemyInRange.Remove(collision.transform);
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.cellInRange.Remove(this.transform);
            enemyHealth.OnEnemyDie -= OnInRangeEnemyDie;
            if (enemyInRange.Count == 0)
            {
                if (enumerator != null)
                {
                    StopCoroutine(enumerator);
                    enumerator = null;
                }
            }
        }
    }
    public Transform ChooseTargetEnemey()
    {
        if (enemyInRange.Count == 0)
        {
            // Debug.LogError("No Target in range");
            return null;
        }
        else
        {
            switch (fireMode)
            {
                case FireMode.First:
                    return CheckAndReturnFirstEnemy();
                case FireMode.Nearest:
                    return CheckAndReturnNearestEnemy();
                case FireMode.Weakest:
                    return CheckAndReturnWeakestEnemy();
                default: return CheckAndReturnFirstEnemy();
            }
        }
    }
    protected Transform CheckAndReturnFirstEnemy()
    {
        Transform p = null;
        for (int i = 0; i < enemyInRange.Count; i++)
        {
            Transform trans = (Transform)enemyInRange[i];
            if (trans == null || trans.GetComponent<EnemyHealth>().Hp <= 0) continue;
            CheckLeftOrRight(p);
            p = trans;
            return p;
        }
        return null;
    }
    protected Transform CheckAndReturnNearestEnemy()
    {
        Transform p = null;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < enemyInRange.Count; i++)
        {
            Transform trans = (Transform)enemyInRange[i];
            if (trans == null || trans.GetComponent<EnemyHealth>().Hp <= 0) continue;

            float distance = Vector3.Distance(trans.position, transform.position);
            //Debug.Log(distance);
            if (distance <= minDistance)
            {
                minDistance = distance;
                p = trans;
            }
        }
        CheckLeftOrRight(p);

        return p;
    }

    protected Transform CheckAndReturnWeakestEnemy()
    {
        Transform p = null;
        float minHp = 1000f;
        for (int i = 0; i < enemyInRange.Count; i++)
        {
            Transform trans = (Transform)enemyInRange[i];
            if (trans == null || trans.GetComponent<EnemyHealth>().Hp <= 0) continue;

            float hp = trans.GetComponent<EnemyHealth>().Hp;
            if (hp < minHp)
            {
                minHp = hp;
                p = trans;
            }
        }
        CheckLeftOrRight(p);
        return p;
    }
    protected void CheckLeftOrRight(Transform targetTransfrom)
    {
        if (targetTransfrom == null) return;
        cellAnimator.direction = transform.position.x > targetTransfrom.position.x ?
                                    Direction.Left :
                                    Direction.Right;
        cellAnimator.transform.localPosition = cellAnimator.direction == Direction.Left ?
                                    revisePosLeft :
                                    revisePosRight;
    }
    public IEnumerator StartAttackEnemy()
    {
        while (true)
        {
            AttackOneTime();
            yield return new WaitForSeconds(atkDuration);
        }
    }
    public virtual void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
        {
            cellStatus = CellStatus.Idle;
            return;
        }
        cellStatus = CellStatus.Attack;
        Invoke("SetDamageToEnemy", atkTime);
    }
    public void SetDamageToEnemy()
    {
        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<EnemyHealth>().TakeDamage(atkDamage, EnemyStatus.Die);
        }
    }
    public void SetDamageToEnemy(EnemyStatus enemyStatus)
    {
        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<EnemyHealth>().TakeDamage(atkDamage, enemyStatus);
        }
    }
    public void OnCellStatusChange(CellStatus cellStatus)
    {
        this.cellStatus = cellStatus;
        if (cellStatus == CellStatus.Die) Destroy(gameObject);
    }

    public void OnDestroy()
    {
        cellAnimator.OnStatusChange -= OnCellStatusChange;
        StopAllCoroutines();
    }

    
}
