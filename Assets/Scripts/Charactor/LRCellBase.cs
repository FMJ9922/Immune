using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LRCellBase : CellBase, LongRangeAttack
{

    protected FireMode fireMode;
    private float reloadTime;
    private bool allowAttack;
    public Slider AtkSlider;//攻击进度条
    public Detector detector;
    public FireModeHandle fireModeHandle;

    public override void InitCell()
    {
        base.InitCell();
        detector = transform.Find("Detector").GetComponent<Detector>();
        detector.GetComponent<CircleCollider2D>().radius = atkRange;
        fireMode = FireMode.Nearest;
        AtkSlider = transform.Find("SliderCanvas").Find("AtkSlider").GetComponent<Slider>();
        AtkSlider.value = 1;
        reloadTime = atkDuration;
        allowAttack = false;
        attackType = AttackType.Other;
        fireModeHandle = transform.Find("FireModeCanvas").GetComponent<FireModeHandle>();
        fireModeHandle.fireMode = fireMode;
        if (transform.position.y < 1f)
        {
            transform.Find("FireModeCanvas").position += new Vector3(0, 1.5f, 0);
        }
        fireModeHandle.OnFireModeChange += ChangeFireMode;
    }
    public void ChangeFireMode(FireMode fireMode)
    {
        this.fireMode = fireMode;
    }
    void FixedUpdate()
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

    }

    public override void StartAction()
    {
        allowAttack = true;
        //Debug.Log("1");
    }

    public override void StopAction()
    {
        allowAttack = false;
    }
    public Transform ChooseTargetEnemey()
    {
        return detector.CheckEnemyArrayList(fireMode, attackType);
    }

    public virtual void AttackOneTime()
    {
        targetEnemy = ChooseTargetEnemey();
        if (targetEnemy == null)
        {
            StopAction();
            OnCellStatusChange(CellStatus.Idle);
            return;
        }
        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        Invoke("FireWeapon", atkTime);

    }
    public virtual void FireWeapon()
    {

    }
    public override void ShowRangePic()
    {
        base.ShowRangePic();
        fireModeHandle.gameObject.SetActive(true);
    }
    public override void CloseRangePic()
    {
        base.CloseRangePic();
        fireModeHandle.gameObject.SetActive(false);
    }
    public override void OnEnemyEnter(Transform enemyTrans)
    {
        base.OnEnemyEnter(enemyTrans);
        EnemyStatus status = enemyTrans.GetComponent<EnemyMotion>().enemyStatus;
        if (status == EnemyStatus.Die || status == EnemyStatus.Engulfed)
        { 
            return; 
        }
        else
        {
            StartAction();
        }

    }
}
