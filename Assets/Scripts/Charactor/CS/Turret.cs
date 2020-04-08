using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 5;//攻击范围
    public string enemyTag = "Enemy";
    public Transform target;//攻击目标
    public float damage =50;//受伤的攻击力

    // public Transform partRotate;//旋转的炮台
    // public float rotSpeed = 10;//旋转速度

    public float attackRateTime = 1; //多少秒攻击一次
    private float timer = 0;


   
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);//更新，每个0.5调用一次

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
       
        else{

            Vector3 dir = target.position - transform.position;
            if (Vector3.Distance(target.position, transform.position) < range)
            {
                //击中目标了
                Attack();
                return;
            }
        }

    }
    private void Attack()
    {
        timer += Time.deltaTime;
        if (timer >= attackRateTime)
        {
            timer = 0;
            //target.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        
    }

private void UpdateTarget()
        //找敌人
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);//找到所有enemy返回数组
        float minDistance = Mathf.Infinity;
        Transform nearestEnemy = null;
        foreach (var enemy in enemies)//在每个enemy去便利
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);//取炮塔和敌人间距离
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;//找到最近的敌人
            }
        }
        if (minDistance < range)//判断最小距离是不是在攻击范围内
        {
            target = nearestEnemy;
           
        }
        else
        {
            target = null;
        }
    }
    private void OnDrawGizmosSelected()//绘制球范围
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);//以塔为中心
    }
   /* private void LockTarget()//旋转方向
    {
        Vector3 dir = target.position - transform.position;//方向
        Quaternion rotation = Quaternion.LookRotation(dir);//朝向
        Quaternion lerpRot = Quaternion.Lerp(partRotate.rotation, rotation, Time.deltaTime * rotSpeed);
        partRotate.rotation = Quaternion.Euler(new Vector3(lerpRot.eulerAngles.x,0 ));
    }*/
}
