using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Detectable
{
    void OnTriggerEnter2D(Collider2D collision);
    void OnTriggerExit2D(Collider2D collision);
    void OnInRangeEnemyDie(Transform enemyTrans);
    Transform CheckEnemyArrayList(FireMode fireMode, AttackType attackType);

}
public class Detector : MonoBehaviour, Detectable
{
    private CellBase cell;
    private void Awake()
    {
        cell = transform.parent.GetComponent<CellBase>();
        transform.localPosition = Vector3.zero;
    }
   
    public Transform CheckEnemyArrayList(FireMode fireMode, AttackType attackType)
    {
        ArrayList enemyInRange = cell.GetEnemyList();
        Transform p = null;
        float minDistance = Mathf.Infinity;
        float minHp = Mathf.Infinity;
        float maxHp = 0f;
        for (int i = 0; i < enemyInRange.Count; i++)
        {
            Transform trans = (Transform)enemyInRange[i];
            if (trans == null) continue;
            EnemyMotion enemyMotion = trans.GetComponent<EnemyMotion>();
            if (enemyMotion.enemyStatus == EnemyStatus.Engulfed) continue;
            else if (enemyMotion.enemyStatus == EnemyStatus.Die && attackType == AttackType.Other) continue;
            switch (fireMode)
            {
                case FireMode.First:
                    {
                        p = trans;
                        return p; 
                    }
                case FireMode.Nearest:
                    {
                        float distance = Vector3.Distance(trans.position, transform.position);
                        if (distance <= minDistance)
                        {
                            minDistance = distance;
                            p = trans;
                        }
                        break;
                    }
                case FireMode.Weakest:
                    {
                        float hp = trans.GetComponent<EnemyHealth>().Hp;
                        if (hp < minHp)
                        {
                            minHp = hp;
                            p = trans;
                        }
                        break;
                    }
                case FireMode.Strongest:
                    {
                        float hp = trans.GetComponent<EnemyHealth>().Hp;
                        if (hp> maxHp)
                        {
                            maxHp = hp;
                            p = trans;
                        }
                        break;
                    }
                default: return null;
            }
        }
        //Debug.Log(p.name);
        return p;
    }

    public void OnInRangeEnemyDie(Transform enemyTrans)
    {
        if (enemyTrans.CompareTag("Enemy"))
        {
            cell.OnInRangeEnemyDie(enemyTrans);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            try
            {
                cell.OnEnemyEnter(collision.transform);
            }
            catch
            {
                Debug.Log("what?"+transform.position);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            cell.OnEnemyExit(collision.transform);
        }
    }
}
