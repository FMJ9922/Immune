using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    private static Dictionary<string, GameObject> CellPfbsDic;
    private static Dictionary<string, GameObject> EnemyPfbsDic;
    private static string cellPath = "Prefab/Cell/{0}Cell";
    private static string enemyPath = "Prefab/Enemy/Enemy{0}";

    public void Awake()
    {
        CellPfbsDic = new Dictionary<string, GameObject>();
        EnemyPfbsDic = new Dictionary<string, GameObject>();
    }
    public static GameObject GetCellPrefab(CellType cellType)
    {
        string cellName = cellType.ToString();
        if (CellPfbsDic.ContainsKey(cellName))
        {
            return CellPfbsDic[cellName];
        }
        else
        {
            string path = string.Format(cellPath, cellName);
            Debug.Log("加载细胞路径："+path);
            GameObject newCell = Resources.Load(path) as GameObject;
            CellPfbsDic.Add(cellName, newCell);
            return CellPfbsDic[cellName];
        }
    }
    public static GameObject GetEnemyPrefab(ActorType actorType)
    {
        string enemyName = actorType.ToString();
        if (EnemyPfbsDic.ContainsKey(enemyName))
        {
            return EnemyPfbsDic[enemyName];
        }
        else
        {
            string path = string.Format(enemyPath, enemyName);
            Debug.Log("加载敌人路径：" + path);
            GameObject newEnemy = Resources.Load(path) as GameObject;
            EnemyPfbsDic.Add(enemyName, newEnemy);
            return EnemyPfbsDic[enemyName];
        }
    }

}
