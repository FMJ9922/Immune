using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    public GameObject[] enemys;
    private float time;//每波间隔时间
    private float times;//波内每个敌人产生间隔
    private float count;//波数
    private float counts;//每波数量

    // Start is called before the first frame update
    void Start()
    {
        time = 2;
        times = 1;
        count = 5;
        counts = 4;
        StartCoroutine(CreateEnemy());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CreateEnemy()
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < counts; j++)
            {
                Instantiate(enemys[Random.Range(0, enemys.Length)], transform.position, Quaternion.identity);//随机生成
                yield return new WaitForSeconds(times);
            }
            yield return new WaitForSeconds(time);
        }
    }
}
