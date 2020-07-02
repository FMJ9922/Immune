using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NormalCellBase : CellBase
{
    public Slider HpSlider;
    public float Hp;
    public Detector detector;
    private float[] damangeCounter = new float[6];

    public override void InitCell()
    {
        enemyInRange = new ArrayList();
        cellAnimator = transform.GetComponentInChildren<CellAnimator>();
        cellAnimator.transform.localPosition = revisePosLeft;
        cellAnimator.OnStatusChange += OnCellStatusChange;
        cellStatus = CellStatus.Idle;
        HpSlider = transform.GetComponentInChildren<Slider>();
    }
    public override void OnEnemyEnter(Transform enemyTrans)
    {
        if (enemyTrans == null) return;
        enemyInRange.Add(enemyTrans);
        EnemyHealth enemyHealth = enemyTrans.GetComponent<EnemyHealth>();
        enemyHealth.cellInRange.Add(this.transform);
        enemyHealth.OnEnemyDie += OnInRangeEnemyDie;
        StartAction(enemyTrans);
    }
    public void StartAction(Transform enemyTrans)
    {
        
        EnemyMotion enemyMotion = enemyTrans.GetComponent<EnemyMotion>();
        if (enemyMotion.enemyStatus != EnemyStatus.Idle) { return; }
        EnemyData enemyData = JsonIO.GetEnemyData(enemyMotion.actorType);
        float p = Random.Range(0.0f, 1.0f);
        //Debug.Log(p + " " + enemyData.rate);
        if (p < enemyData.rate)
        {
            Hp -= enemyData.atk;
            damangeCounter[(int)enemyMotion.actorType - 20] += enemyData.atk;
            HpSlider.value = Hp/100F;
            Debug.Log("受到了来自"+enemyTrans.name+"的伤害：-"+enemyData.atk);
            if (Hp <= 0)
            {

                //ChangeToEnemy();
                OnDie();
            }
        }
    }

    public override void StartAction()
    {

    }
    public override void StopAction()
    {
        
    }
    public void ChangeToEnemy()
    {
        float max = 0;
        ActorType actorType = ActorType.TH;
        for(int i = 0;i< damangeCounter.Length; i++)
        {
            if (damangeCounter[i] > max)
            {
                max = damangeCounter[i];
                actorType = (ActorType)i;
            } 
        }
        LevelManager.Instance.CreateOneEnemy(actorType);
    }
}
