using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRCellBase : CellBase, ShortRangeAttack
{
    protected FireMode fireMode;
    protected  float reloadTime;
    protected bool allowAttack;
    protected Slider AtkSlider;//攻击进度条
    protected  Detector detector;
    public FireModeHandle fireModeHandle;
    public override void InitCell()
    {
        base.InitCell();
        detector = transform.Find("Detector").GetComponent<Detector>();
        detector.GetComponent<CircleCollider2D>().radius = atkRange;
        fireMode = FireMode.First;
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
    protected virtual void FixedUpdate()
    {
        if ( cellStatus == CellStatus.Die) { return; }

        AtkSlider.value = Mathf.Clamp(reloadTime/ atkDuration,0,1);
        if (reloadTime <atkDuration)
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
            OnCellStatusChange(CellStatus.Idle);
            allowAttack = false;
            return;
        }
        cellAnimator.CleanFrameData();
        OnCellStatusChange(CellStatus.Attack);
        Invoke("SetDamageToEnemy", atkTime);
    }
    public void SetDamageToEnemy()
    {
        if (targetEnemy != null)
        {
            float coefficient = JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType);
            targetEnemy.GetComponent<EnemyHealth>().TakeDamage(atkDamage* coefficient, false);
        }
    }
    public void SetDamageToEnemy(AttackType attackType)
    {
        if (targetEnemy != null&&attackType==AttackType.Swallow)
        {
            //Debug.Log("eat");
            float coefficient = JsonIO.GetCoefficiet(cellType, targetEnemy.GetComponent<EnemyMotion>().enemyType);
            targetEnemy.GetComponent<EnemyHealth>().TakeDamage(atkDamage* coefficient, true);
        }
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
}
