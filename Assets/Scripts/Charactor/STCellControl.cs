using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STCellControl : AssistCellBase
{
    public override void Produce()
    {
        
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (enemyInRange.Count==0)
            {
                CheckLeftOrRight(collision.transform);
                cellAnimator.reverse = false;
                OnCellStatusChange(CellStatus.Change);

                if (enumerator == null)
                {
                    enumerator = StartProduce();
                    StartCoroutine(enumerator);
                }
            }
            enemyInRange.Add(collision.transform);
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.cellInRange.Add(this.transform);
            enemyHealth.OnEnemyDie += OnInRangeEnemyDie;


        }

    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyInRange.Remove(collision.transform);
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.cellInRange.Remove(this.transform);
            enemyHealth.OnEnemyDie -= OnInRangeEnemyDie;
            if (enemyInRange.Count == 0)
            {
                cellAnimator.reverse = true;
                OnCellStatusChange(CellStatus.Change);
                if (enumerator != null)
                {
                    StopCoroutine(enumerator);
                    enumerator = null;
                }
            }
        }
    }
}
