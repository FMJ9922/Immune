using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System;
using System.IO;

[System.Serializable]
public class GameData
{
    public LevelData[] levelDatas;
}

[System.Serializable]
public class LevelData
{
    public string levelName;
    public Wave[] waves;
    /*public WayPoint[] wayPoints;*/
    public int[] allowCellType;
    public int[,] mapType;
    public ScoreRequest[] scoreRequests;
}

[System.Serializable]
public class Wave//敌人种类，数量，生成时间间隔，下一波时间间隔，寻路机制，起点x，y,终点x,y,速度
{
    public int enemyType;
    public int enemyNum;
    public float initDuration;
    public float nextWaveDuration;
    public int findPathType;
    public int startX, startY;
    public int endX, endY;
    public float speed;

}
[System.Serializable]
public class ScoreRequest
{
    public ScoreType scoreType;//得分类型
    public int requestNum;//得分要求
    public int actualNum = 0;//实际数量
}
[System.Serializable]
public class CellData
{
    public string name;
    public float atkRange;
    public float atkDamage;
    public float atkTime;
    public float atkDuration;
    public float initCost;
    public string introduce;
    public string type;
    public string ability;
    public float[] coefficient;
}
[System.Serializable]
public class EnemyData
{
    public string name;
    public string type;
    public string ability;
    public float Hp;
    public float atk;
    public float rate;
    public float speed;
    public string introduce;
}

public class JsonIO : MonoBehaviour
{
    public static GameData gameData;
    private static LevelData levelData;
    public static int lastLevelNum;
    public static CellData[] cellDatas;
    public static EnemyData[] enemyDatas;

    public static void InitLevelData(int index)
    {
        levelData = gameData.levelDatas[index-1];
        Debug.Log("Load Level " + index);
    }

    public static string GetLevelName()
    {
        return levelData.levelName;
    }

    public static Wave[] GetWaves()
    {
        return levelData.waves;
    }
    public static Vector4[] GetDefaultPath()
    {
        Vector4[] vectors = new Vector4[levelData.waves.Length];
        for (int i = 0; i < levelData.waves.Length; i++)
        {
            vectors[i].x = levelData.waves[i].startX;
            vectors[i].y = levelData.waves[i].startY;
            vectors[i].z = levelData.waves[i].endX;
            vectors[i].w = levelData.waves[i].endY;
        }
        return vectors;
    }
    public static int[] GetAllowCellType()
    {
        return levelData.allowCellType;
    }
    public static CellData GetCellData(CellType cellType)
    {
        return cellDatas[(int)cellType];
    }
    public static EnemyData GetEnemyData(ActorType actorType)
    {
        //Debug.Log((int)actorType);
        return enemyDatas[(int)actorType - 20];
    }
    public static int[,] GetMap()
    {
        return levelData.mapType;
    }
    public static void InitCellData()
    {
        TextAsset t = (TextAsset)Resources.Load("Data/CellData", typeof(TextAsset));
        string jsonString = t.ToString();
        var dict = (Dictionary<string, object>)Json.Deserialize(jsonString);
        List<System.Object> CellDataList = (List<System.Object>)dict["lists"];
        cellDatas = new CellData[CellDataList.Count];
        for(int i = 0; i < CellDataList.Count; i++)
        {
            var dict2 = (Dictionary<string, object>)CellDataList[i];
            cellDatas[i] = new CellData();
            cellDatas[i].name = (string)dict2["Name"];
            cellDatas[i].atkDamage = (float)(double)dict2["AtkDamage"];
            cellDatas[i].atkRange = (float)(double)dict2["AtkRange"];
            cellDatas[i].atkTime = (float)(double)dict2["AtkTime"];
            cellDatas[i].atkDuration = (float)(double)dict2["AtkDuration"];
            cellDatas[i].initCost = (float)(double)dict2["InitCost"];
            cellDatas[i].introduce = (string)dict2["Introduce"];
            cellDatas[i].type = (string)dict2["Type"];
            cellDatas[i].ability = (string)dict2["Ability"];
            cellDatas[i].coefficient = PraseFloat((List<object>)dict2["Coefficient"]);
        }
    }
    public static void InitEnemyData()
    {
        TextAsset t = (TextAsset)Resources.Load("Data/EnemyData", typeof(TextAsset));
        string jsonString = t.ToString();
        var dict = (Dictionary<string, object>)Json.Deserialize(jsonString);
        List<System.Object> EnemyDataList = (List<System.Object>)dict["lists"];
        enemyDatas = new EnemyData[EnemyDataList.Count];
        for (int i = 0; i < EnemyDataList.Count; i++)
        {
            var dict2 = (Dictionary<string, object>)EnemyDataList[i];
            enemyDatas[i] = new EnemyData();
            enemyDatas[i].name = (string)dict2["Name"];
            enemyDatas[i].type = (string)dict2["Type"];
            enemyDatas[i].ability = (string)dict2["Ability"];
            enemyDatas[i].Hp = (float)(double)dict2["Hp"];
            enemyDatas[i].atk = (float)(double)dict2["Atk"];
            enemyDatas[i].rate = (float)(double)dict2["Rate"];
            enemyDatas[i].speed = (float)(double)dict2["Speed"];
            enemyDatas[i].introduce = (string)dict2["Introduce"];
        }
    }
    public static void InitGameData()
    {
        Debug.Log("GameData Init");
        TextAsset t = (TextAsset)Resources.Load("Data/GameData", typeof(TextAsset));
        string jsonString = t.ToString();
        var dict = (Dictionary<string, object>)Json.Deserialize(jsonString);
        List<System.Object> levelDataList = (List<System.Object>)dict["lists"];
        lastLevelNum = levelDataList.Count;
        LevelData[] levelDatas = new LevelData[lastLevelNum];

        for (int i = 0; i < lastLevelNum; i++)
        {
            levelDatas[i] = GetLevelData(i, levelDataList);
        }
        gameData = new GameData();
        gameData.levelDatas = levelDatas;
    }
    public static ScoreRequest[] GetScoreRequest()
    {
        return levelData.scoreRequests;
    }
    public static LevelData GetLevelData(int index, List<System.Object> levelDataList)
    {

        //Debug.Log("InitLevel " + index + " Data");

        if (index < 0 || index > lastLevelNum)
        {
            Debug.Log("invalid level index");
            return null;
        }

        LevelData levelData = new LevelData();
        var dict2 = (Dictionary<string, object>)levelDataList[index];
        var levelName = (string)dict2["levelName"];
        var waveList = (List<object>)dict2["waves"];
        var allowCellList = (List<object>)dict2["allowCell"];
        var mapTypeList = (List<object>)dict2["mapType"];
        var ScoreRequest = (List<object>)dict2["scoreRequests"];
        /*Debug.Log(mapTypeList.ToString());
        Debug.Log(ScoreRequest.ToString());*/
        levelData.levelName = levelName;
        levelData.waves = PraseWave(waveList);
        levelData.allowCellType = PraseInt(allowCellList);
        levelData.mapType = PraseMap(mapTypeList);
        levelData.scoreRequests = PraseScoreRequest(ScoreRequest);

       
        return levelData;
    }

    private static ScoreRequest[] PraseScoreRequest(List<object> scoreRequest)
    {
        ScoreRequest[] _scoreRequests = new ScoreRequest[3];
        for(int i = 0; i < 3; i++)
        {
            _scoreRequests[i] = new ScoreRequest();
            
            var singleRequest = (List<System.Object>)scoreRequest[i];
            for (int j = 0; j < 2; j++)
            {
                switch (j)
                {
                    case 0:
                        int scoretype = Convert.ToInt16(singleRequest[j]);
                        _scoreRequests[i].scoreType = (ScoreType)scoretype;
                        break;
                    case 1:
                        _scoreRequests[i].requestNum = Convert.ToInt32(singleRequest[j]);
                        break;
                }
            }
            _scoreRequests[i].actualNum = 0;
        }
        return _scoreRequests;
    }

    private static int[,] PraseMap(List<object> mapTypeList)
    {
        int[,] map = new int[16, 9];
        for(int i = 0; i < 9; i++)
        {
            var row = (List<System.Object>)mapTypeList[i];
            for (int j = 0; j < 16; j++)
            {
                map[j,8-i] = Convert.ToInt16(row[j]);
            }
        }

        return map;
    }

    private static Wave[] PraseWave(List<object> wavelist)
    {
        Wave[] _waves = new Wave[wavelist.Count];
        for (int i = 0; i < wavelist.Count; i++)
        {
            var singleWave = (List<System.Object>)wavelist[i];
            //Debug.Log((double)singleWave[3]);
            _waves[i] = new Wave();
            for (int j = 0;j< 10; j++)
            {
                switch (j)
                {
                    case 0:
                        _waves[i].enemyType = Convert.ToInt16(singleWave[j]);
                        break;
                    case 1:
                        _waves[i].enemyNum = Convert.ToInt16(singleWave[j]);
                        break;
                    case 2:
                        _waves[i].initDuration = (float)(double)singleWave[j];
                        break;
                    case 3:
                        _waves[i].nextWaveDuration = (float)(double)singleWave[j];
                        //Debug.Log(_waves[i].nextWaveDuration);
                        break;
                    case 4:
                        _waves[i].findPathType = Convert.ToInt16(singleWave[j]);
                        break;
                    case 5:
                        _waves[i].startX = Convert.ToInt16(singleWave[j]);
                        break;
                    case 6:
                        _waves[i].startY = Convert.ToInt16(singleWave[j]);
                        break;
                    case 7:
                        _waves[i].endX = Convert.ToInt16(singleWave[j]);
                        break;
                    case 8:
                        _waves[i].endY = Convert.ToInt16(singleWave[j]);
                        break;
                    case 9:
                        _waves[i].speed = (float)(double)singleWave[j];
                    break;
                }
                
            }
        }
        return _waves;
    }
    private static int[] PraseInt(List<object> intList)
    {
        int[] ret = new int[intList.Count];
        for(int i = 0; i < intList.Count; i++)
        {
            ret[i] = Convert.ToInt16(intList[i]);
        }
        return ret;
    }
    private static float[] PraseFloat(List<object> floatList)
    {
        float[] ret = new float[floatList.Count];
        for (int i = 0; i < floatList.Count; i++)
        {
            ret[i] = (float)(double)(floatList[i]);
        }
        return ret;
    }
    public static float GetCoefficiet(CellType cellType,EnemyType enemyType)
    {
        //Debug.Log(GetCellData(cellType).name);
        return GetCellData(cellType).coefficient[(int)enemyType];
    }
    /*public static ActorType ActorTypeToEnemyType(EnemyType enemyType)
    {
        ActorType actorType;
        switch (enemyType)
        {
            case EnemyType.BD:
            case EnemyType.JSC:
            case EnemyType.XJ:
        }

    }*/
}
