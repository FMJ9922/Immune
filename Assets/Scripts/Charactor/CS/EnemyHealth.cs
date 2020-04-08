using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float InitHealth = 100;//初始生命值
    private float damage;
    private EnemyAnimator enemyAnimator;
    public ArrayList cellInRange;

    public delegate void EnemyDie(Transform enmeyTrans);
    public EnemyDie OnEnemyDie;

    public float Hp
    {
        get;
        private set;
    }

    private Slider hpSlider;//血条
//public GameObject explosionEffect;
    void Start()
    {
        cellInRange = new ArrayList();
          Hp = InitHealth;
        hpSlider = GetComponentInChildren<Slider>();
        enemyAnimator = transform.GetComponentInChildren<EnemyAnimator>();
    }

    void Update()
    {

    }
   
    public void TakeDamageInSeconds(float damage,float time)
    {
       // Debug.Log(damage + " " + time);
        Invoke("TakeDamage", time);
        this.damage = damage; 
    }
    private void TakeDamage()
    {
        
        if (Hp <= 0) return;
        Hp -= this.damage;
        hpSlider.value = (float)Hp / InitHealth;
        //Debug.Log(Hp);
        if (Hp <= 0)
        {
            enemyAnimator.OnChangeCellStatus(EnemyStatus.Die);
            Destroy(hpSlider.gameObject);
            Die();
        }
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
        Destroy(this.gameObject,2f);
    }

}
