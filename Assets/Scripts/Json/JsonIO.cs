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
    public WayPoint[] wayPoints;
}

[System.Serializable]
public class Wave
{
    public int enemyType;
    public int enemyNum;
    public float initDuration;
    public float nextWaveDuration;
}

[System.Serializable]
public class WayPoint
{
    public Vector3 position;
}


public class JsonIO : MonoBehaviour
{
    public static GameData gameData;
    private static LevelData levelData;
    public static int lastLevelNum;

    public static void InitLevelData(int index)
    {
        levelData = gameData.levelDatas[index - 1];
        //Debug.Log("Load Level " + index);
    }

    public static string GetLevelName()
    {
        return levelData.levelName;
    }

    public static Wave[] GetWaves()
    {
        return levelData.waves;
    }

    public static WayPoint[] GetWayPoints()
    {
        return levelData.wayPoints;
    }

    public static void InitGameData()
    {
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
        var wayPointList = (List<object>)dict2["wayPoints"];

        levelData.levelName = levelName;
        levelData.waves = PraseWave(waveList);
        levelData.wayPoints = PraseWayPoint(wayPointList);

       
        return levelData;
    }
    private static Wave[] PraseWave(List<object> wavelist)
    {
        Wave[] _waves = new Wave[wavelist.Count];
        for (int i = 0; i < wavelist.Count; i++)
        {
            var singleWave = (List<System.Object>)wavelist[i];
            //Debug.Log((double)singleWave[3]);
            _waves[i] = new Wave();
            for (int j = 0;j< 4; j++)
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
                }
                
            }
        }
        return _waves;
    }

    private static WayPoint[] PraseWayPoint(List<object> waypointlist)
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
    }
}
