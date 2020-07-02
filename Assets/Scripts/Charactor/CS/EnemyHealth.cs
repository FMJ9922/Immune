using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float InitHealth;//初始生命值
    private EnemyAnimator enemyAnimator;
    public ArrayList cellInRange;

    public delegate void EnemyDie(Transform enmeyTrans);
    public EnemyDie OnEnemyDie;
    EnemyMotion enemyMotion;

    public float Hp
    {
        get;
        private set;
    }
    private float DieTimeConter;

    private Slider hpSlider;//血条
                            //public GameObject explosionEffect;
    void Start()
    {
       
        enemyMotion = transform.GetComponent<EnemyMotion>();
        InitHealth = JsonIO.GetEnemyData(enemyMotion.actorType).Hp;
        cellInRange = new ArrayList();
        Hp = InitHealth;
        hpSlider = GetComponentInChildren<Slider>();
        enemyAnimator = transform.GetComponentInChildren<EnemyAnimator>();
    }

    /*public void TakeDamageInSeconds(float damage,float time)
    {
       // Debug.Log(damage + " " + time);
        Invoke("TakeDamage", time);
        this.damage = damage; 
    }*/
    public void TakeDamage(float damage, bool swallow)
    {
        //if (Hp <= 0) return;
        Hp -= damage* enemyMotion.GetCoefficient();
        hpSlider.value = (float)Hp / InitHealth;
        
        if (Hp <= 0)
        {
            EnemyStatus _enemyStatus = swallow ? EnemyStatus.Engulfed : EnemyStatus.Die;
            enemyAnimator.OnChangeEnemyStatus(_enemyStatus);
            enemyMotion.enemyStatus = _enemyStatus;
            hpSlider.gameObject.SetActive(false);
            //Destroy(hpSlider.gameObject);
            if (_enemyStatus == EnemyStatus.Engulfed)
            {
                StartCoroutine(MyTool.DoRotate(transform, true, -50f, 1f));
                StartCoroutine(MyTool.DoScale(transform, 0.12f, 1.0f));
                Destroy(enemyMotion.AntiTrans.gameObject);
                Die();
                SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_enemy_dead);
                return;

            }
            else if(_enemyStatus ==EnemyStatus.Die)
            {
                //transform.GetComponent<CircleCollider2D>().enabled = false;
                enemyMotion.StopAllCoroutines();
                //Debug.Log("die");
                enemyMotion.GetSlowDown(0.5f, 10000f);
            }
        }
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_enemy_hit1);
    }
    void Die()
    {
        if (cellInRange.Count != 0)
        {
            if (OnEnemyDie != null)
            {
                //Debug.Log("Des");
                OnEnemyDie(this.transform);
            }
        }
        DieTimeConter = 2f;
        Destroy(this.gameObject, 2f);
    }

}
