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
        EnemyData enemyData = JsonIO.GetEnemyData(enemyMotion.actorType);
        float p = Random.Range(0, 1);
        if (p < enemyData.rate)
        {
            Hp -= enemyData.atk;
            damangeCounter[(int)enemyMotion.actorType - 20] += enemyData.atk;
            HpSlider.value = Hp;
            if (Hp <= 0)
            {
                ChangeToEnemy();
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
        ActorType actorType;
        for(int i = 0;i< damangeCounter.Length; i++)
        {
            if (damangeCounter[i] > max)
            {
                max = damangeCounter[i];
                actorType = (ActorType)i;
            } 
        }
    }
}
