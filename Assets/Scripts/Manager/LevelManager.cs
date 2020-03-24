using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] enemys;
    private Wave[] waves;
    // Start is called before the first frame update
    void Start()
    {
        waves = JsonIO.GetWaves();
        StartCoroutine(CreateEnemy());
        //Check();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private IEnumerator CreateEnemy()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            //Debug.Log(i+" "+waves[i].enemyNum+" "+ waves[i].initDuration+" "+ waves[i].nextWaveDuration);
            for (int j = 0; j < waves[i].enemyNum; j++)
            {
                Instantiate(enemys[waves[i].enemyType], transform.position, Quaternion.identity);//随机生成
                yield return new WaitForSeconds(waves[i].initDuration);
            }
            yield return new WaitForSeconds(waves[i].nextWaveDuration);
        }
    }
    void Check()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            Debug.Log(i + " "+ waves[i].nextWaveDuration);
            
        }
    }
}
