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
}

[System.Serializable]
public class Wave
{
    public int enemyType;
    public int enemyNum;
    public float initDuration;
    public float nextWaveDuration;
    public int findPathType;
    public int startX, startY;
    public int endX, endY;

}

[System.Serializable]
public class CellData
{
    public float atkRange;
    public float atkDamage;
    public float atkTime;
    public float atkDuration;
    public float initCost;
}

public class JsonIO : MonoBehaviour
{
    public static GameData gameData;
    private static LevelData levelData;
    public static int lastLevelNum;
    public static CellData[] cellDatas;

    public static void InitLevelData(int index)
    {
        levelData = gameData.levelDatas[index - 1];
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
            cellDatas[i].atkDamage = (float)(double)dict2["AtkDamage"];
            cellDatas[i].atkRange = (float)(double)dict2["AtkRange"];
            cellDatas[i].atkTime = (float)(double)dict2["AtkTime"];
            cellDatas[i].atkDuration = (float)(double)dict2["AtkDuration"];
            cellDatas[i].initCost = (float)(double)dict2["InitCost"];
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

        levelData.levelName = levelName;
        levelData.waves = PraseWave(waveList);
        levelData.allowCellType = PraseInt(allowCellList);
        levelData.mapType = PraseMap(mapTypeList);

       
        return levelData;
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
            for (int j = 0;j< 9; j++)
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
    /*private static WayPoint[] PraseWayPoint(List<object> waypointlist)
    {
        WayPoint[] _wayPoints = new WayPoint[waypointlist.Count];
        for (int i = 0; i < waypointlist.Count; i++)
        {
            var aWayPoint = (List<System.Object>)waypointlist[i];
            _wayPoints[i] = new WayPoint();
            _wayPoints[i].position = new Vector3(0,0,0);
            //Debug.Log((double)aWayPoint[0]);
            double x = 0, y = 0, z = 0;
            for (int j = 0; j < 2; j++)
            {
                switch (j)
                {
                    case 0:
                        x = ((double)aWayPoint[j]);
                        break;
                    case 1:
                        y = ((double)aWayPoint[j]);
                        break;
                    case 2:
                        z = ((double)aWayPoint[j]);
                        break;

                }
            }
            _wayPoints[i].position = new Vector3((float)x, (float)y, (float)z);

        }
        return _wayPoints;
    }*/
}
