using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistCellBase : CellBase,Produce
{
    protected CellData cellData;
    protected float atkRange;
    protected float atkDamage;
    protected float atkDuration;
    protected ArrayList enemyInRange;
    public Vector3 revisePosLeft;
    public Vector3 revisePosRight;
    protected IEnumerator enumerator;
    protected CellAnimator cellAnimator;

    public override void InitCell()
    {
        cellData = JsonIO.GetCellData(cellType);
        atkDuration = cellData.atkDuration;
        atkRange = cellData.atkRange;
        atkDamage = cellData.atkDamage;
        enemyInRange = new ArrayList();
        transform.GetComponent<CircleCollider2D>().radius = atkRange;
        cellAnimator = transform.GetComponentInChildren<CellAnimator>();
        cellAnimator.transform.localPosition = revisePosLeft;
        this.rangePicManage = transform.GetComponentInChildren<RangePicManage>();
        cellAnimator.OnStatusChange += OnCellStatusChange;
        enumerator = null;
        cellStatus = CellStatus.Idle;
    }
    void Start()
    {
        InitCell();
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
    protected virtual void OnTriggerEnter2D(Collider2D collision)
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


            OnCellStatusChange(CellStatus.Produce);
            if (enumerator == null)
            {
                enumerator = StartProduce();
                StartCoroutine(enumerator);
            }
        }

    }

    protected IEnumerator StartProduce()
    {
        while (true)
        {
            Produce();
            yield return new WaitForSeconds(atkDuration);
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
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyInRange.Remove(collision.transform);
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.cellInRange.Remove(this.transform);
            enemyHealth.OnEnemyDie -= OnInRangeEnemyDie;
            if (enemyInRange.Count == 0)
            {
                OnCellStatusChange(CellStatus.Idle);
                if (enumerator != null)
                {
                    StopCoroutine(enumerator);
                    enumerator = null;
                }
            }
        }
    }

    public virtual void Produce()
    {

    }
}
