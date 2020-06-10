using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMotion : MonoBehaviour
{

    private float speed = 2f;
    protected ArrayList enemyInRange;
    public float damage;
    private Transform target;//目标
   

   // private float distanceArriveTarget = 1;

    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.LookAt(target.transform.position);//面向目标位置
        Vector2 dir = target.transform.position - transform.position;
        transform.Translate(dir * speed * Time.deltaTime);//移动
       

    }
    void Start()
    {

        enemyInRange = new ArrayList();
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Destroy(this.gameObject);
            enemyInRange.Add(collision.transform);
            //collision.transform.localScale *= 2;
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            enemyHealth.cellInRange.Add(this.transform);
           enemyHealth.OnEnemyDie += OnTriggerEnemyDie;
            SetDamageToEnemy();
            Debug.Log("触碰");
        }

    }
    protected void OnTriggerEnemyDie(Transform enemyTrans)
    {
      enemyTrans.GetComponent<EnemyHealth>().OnEnemyDie -= OnTriggerEnemyDie;
        if (enemyInRange.Contains(enemyTrans))
        {
            enemyInRange.Remove(enemyTrans);
        }
    }

    public void SetDamageToEnemy()
    {


        //Debug.Log(trans.name);
        if (target != null && target.GetComponent<EnemyHealth>().Hp > 0)
        {
            Debug.Log("攻击");
            target.GetComponent<EnemyHealth>().TakeDamage(damage, false);
            target.GetComponent<EnemyMotion>().GetSlowDown(0.5f, 3f);
            gameObject.SetActive(false);
        }

    }
}
