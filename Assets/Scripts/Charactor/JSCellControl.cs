using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JSCellControl : SRCellBase
{
    public Transform eatTrans;
    public Slider ProduceSlider;
    public GameObject product;
    public Transform InitPos;
    private float eatTimes = 0;

    private void Start()
    {
        attackType = AttackType.Swallow;
    }
    protected override void FixedUpdate()
    {
        if (cellStatus == CellStatus.Die) { return; }

        AtkSlider.value = Mathf.Clamp(reloadTime / atkDuration, 0, 1);
        if (reloadTime < atkDuration)
        {
            reloadTime += Time.deltaTime;
        }
        else if (allowAttack)
        {
            AttackOneTime();
            reloadTime = 0;
        }

        ProduceSlider.value = Mathf.Clamp(eatTimes / 5, 0, 1);
        if (eatTimes >=5)
        {
            Produce();
            eatTimes = 0;

        }
    }
    public override void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();

        if (targetEnemy == null)
        {
            OnCellStatusChange(CellStatus.Idle);
            return;
        }
        if (targetEnemy.GetComponent<EnemyHealth>().Hp <= atkDamage* JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType))
        {
            cellAnimator.CleanFrameData();
            OnCellStatusChange(CellStatus.SpecialAbility);
            targetEnemy.GetComponent<EnemyMotion>().TargetPoint = eatTrans.position;
            SetDamageToEnemy(attackType);
            cellAnimator.CleanFrameData();
            eatTimes++;
        }
        else
        {
            cellAnimator.CleanFrameData();
            OnCellStatusChange(CellStatus.Attack);
            Invoke("SetDamageToEnemy", atkTime);
        }
    }
    private void Produce()
    {
        LevelManager.Instance.AddPoints(PointsType.KangYuan,1);
        GameObject newproduct = Instantiate(product, transform);
        newproduct.transform.position = InitPos.position;
    }
    public override void OnEnemyEnter(Transform enemyTrans)
    {
        base.OnEnemyEnter(enemyTrans);
        EnemyStatus status = enemyTrans.GetComponent<EnemyMotion>().enemyStatus;
        if (status == EnemyStatus.Engulfed)
        {
            return;
        }
        else
        {
            StartAction();
        }

    }
}
