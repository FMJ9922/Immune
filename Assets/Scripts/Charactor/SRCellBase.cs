using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRCellBase : CellBase, ShortRangeAttack
{
    private float atkRange;//攻击范围
    private float atkDamage;//攻击伤害
    private float atkDuration;//冷却时间
    private float atkTime;//攻击时长
    private bool isAttack;//是否正在攻击
    private CellData cellData;
    private ArrayList enemyInRange;//范围内的敌人
    private Transform targetEnemy;//目标敌人
    private CellAnimator cellAnimator;
    public Vector3 revisePosLeft = new Vector3(-0.12f, 0.4f, 0);
    public Vector3 revisePosRight = new Vector3(0.22f, 0.4f, 0);

    

    public override void InitCell()
    {
        cellData = JsonIO.GetCellData(cellType);
        isAttack = false;
        atkDamage = cellData.atkDamage;
        atkDuration = cellData.atkDuration;
        atkRange = cellData.atkRange;
        atkTime = cellData.atkTime;
        transform.GetComponent<CircleCollider2D>().radius = atkRange;
        enemyInRange = new ArrayList();
        cellAnimator = transform.GetComponentInChildren<CellAnimator>();
        cellAnimator.transform.localPosition = revisePosLeft;
    }
    void Start()
    {
        InitCell();
        InvokeRepeating("AttackOneTime", 0, atkDuration);
        AttackOneTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (enemyInRange.Count == 0)
            {
                if (transform.position.x > collision.transform.position.x)
                {
                    cellAnimator.direction = Direction.Left;
                    cellAnimator.transform.localPosition = revisePosLeft;

                }
                else
                {
                    cellAnimator.direction = Direction.Right;
                    cellAnimator.transform.localPosition = revisePosRight;
                }
            }
            enemyInRange.Add(collision.transform);
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.cellInRange.Add(this.transform);
            enemyHealth.OnEnemyDie += OnInRangeEnemyDie;
            isAttack = true;
            //Debug.Log(collision.name);
        }

    }
    private void OnInRangeEnemyDie(Transform enemyTrans)
    {
        enemyTrans.GetComponent<EnemyHealth>().OnEnemyDie -= OnInRangeEnemyDie;
        if (enemyInRange.Contains(enemyTrans))
        {
            enemyInRange.Remove(enemyTrans);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyInRange.Remove(collision.transform);
        EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
        enemyHealth.cellInRange.Remove(this.transform); 
        enemyHealth.OnEnemyDie -= OnInRangeEnemyDie;

    }
    public Transform ChooseTargetEnemey()
    {
        if (enemyInRange.Count == 0)
        {
            isAttack = false;
            return null;
        }
        else
        {
            return CheckAndReturnNearestEnemy();
        }
    }

    private Transform CheckAndReturnNearestEnemy()
    {
        Transform p = null;
        for (int i = 0; i < enemyInRange.Count; i++)
        {
            Transform trans = (Transform)enemyInRange[i];
            float minDistance = Mathf.Infinity;
            if (trans == null) return null;
            if (trans.GetComponent<EnemyHealth>().Hp > 0)
            {
                if (transform.position.x > trans.position.x)
                {
                    float distance = Vector3.Distance(trans.position, transform.position + new Vector3(-1, 0, 0));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        cellAnimator.direction = Direction.Left;
                        cellAnimator.transform.localPosition = revisePosLeft;
                        p = trans;
                    }
                }
                else
                {
                    float distance = Vector3.Distance(trans.position, transform.position + new Vector3(1, 0, 0));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        cellAnimator.direction = Direction.Right;
                        cellAnimator.transform.localPosition = revisePosRight;
                        p = trans;
                    }
                }
            }
        }
        return p;
    }
    public void AttackOneTime()
    {
        if (!isAttack) return;
        cellAnimator.OnChangeCellStatus(CellStatus.Attack);
        Invoke("SetDamageToEnemy", atkTime);
    }
    public void SetDamageToEnemy()
    {
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<EnemyHealth>().TakeDamageInSeconds(atkDamage, 0);
        }
        else
        {
            targetEnemy = null;
            return;
        }
    }

}
