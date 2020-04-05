using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float InitHealth = 100;//初始生命值
    private float hp;//当前生命值
    private Slider hpSlider;//血条
//public GameObject explosionEffect;
    void Start()
    {
       hp = InitHealth;
        hpSlider = GetComponentInChildren<Slider>();
    }

    void Update()
    {

    }
    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = (float)hp / InitHealth;
        if (hp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
      //  GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        //Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }

}
