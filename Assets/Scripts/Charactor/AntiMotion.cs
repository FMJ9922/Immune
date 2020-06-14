using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMotion : MonoBehaviour
{

    private float speed = 0.7f;
    public float damage;
    private Transform target;//目标
    //public new Vector3 transform;

 //   private float distanceArriveTarget = 1;
    

    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            if (LevelManager.IsOutOfMapEdge(transform))
            {
                Destroy(this.gameObject);
            }
            return;
        }

        transform.LookAt(target.position);//面向目标位置
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += new Vector3(dir.x, dir.y, 0) * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, GetRenderDepth(transform.position.x, transform.position.y));

    }
    public float GetRenderDepth(float x, float y)
    {
        return -(8.5f - y + x - 0.5f) * 0.001f;
    }
    void Start()
    {

    }
    private void OnDestroy()
    {
        //Debug.Log("?");
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")&&collision.transform==target)
        {
            SetDamageToEnemy();
        }

    }

    public void SetDamageToEnemy()
    {
        if (target != null && target.GetComponent<EnemyHealth>().Hp > 0)
        {
            //Debug.Log(damage);
            target.GetComponent<EnemyHealth>().TakeDamage(damage, false);
            target.GetComponent<EnemyMotion>().GetSlowDown(0.5f, 10f);
            Destroy(gameObject);
        }
        if(target.GetComponent<EnemyMotion>().enemyStatus ==EnemyStatus.Die
            || target.GetComponent<EnemyMotion>().enemyStatus == EnemyStatus.Engulfed)
        {
            
            Destroy(gameObject);
        }

    }
}
