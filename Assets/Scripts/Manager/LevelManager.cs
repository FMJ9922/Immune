﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points
{
    public float DeployPoints;
    public float LinbaPoints;
    public float KangYuanPoints;
    public Points(float value1,float value2,float value3)
    {
        DeployPoints = value1;
        LinbaPoints = value2;
        KangYuanPoints = value3;
    }
    public void AddPoint(PointsType pointsType,float value)
    {

        switch(pointsType)
        {
            case PointsType.Deploy:
                {
                    SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_gain_bushu);
                    DeployPoints += value;
                    break;
                }
            case PointsType.LinBa:
                {
                    SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_gain_linba);
                    LinbaPoints += value;
                    break;
                }
            case PointsType.KangYuan :
                {
                    SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_gain_kangti);
                    KangYuanPoints += value;
                    break;
                }

        }
    }

    public bool SpendPoint(PointsType pointsType, float value)
    {

        switch (pointsType)
        {
            case PointsType.Deploy:
                {
                    return CheckLowerThanMin(ref DeployPoints,value);
                }
            case PointsType.LinBa:
                {
                    return CheckLowerThanMin(ref LinbaPoints, value);
                }
            case PointsType.KangYuan:
                {
                    return CheckLowerThanMin(ref KangYuanPoints, value);
                }

        }
        return false;
    }
    private bool CheckLowerThanMin(ref float point,float value)
    {
        if (value <= point)
        {
            point -= value;
            return true;
        }
        else
        {
            //LoggerManager.Instance.ShowOneLog("点数不足！无法放置");
            return false;
        }
    }

}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } = null;
    public GameObject normalCell;
    public GameObject redCell;
    private Wave[] waves;

    public AStarNode[,] AllNodeGroup;
    public int MapWidth = 16;
    public int MapHeight = 9;
    public int[,] mapType;
    public int navigationSize;
    public GameObject EnemyGroup;
    public GameObject Map;
    public GameObject Node;
    public Transform drawRoute;
    public int curWave = 0;
    private bool finishCreate;
    public Points levelPoints;
    public Transform CountDown;
    private ScoreRequest[] levelRequest = new ScoreRequest[3];
    public delegate void ScoreChange(ScoreRequest[] levelRequest);
    public static event ScoreChange OnScoreChange;

    public GameObject StartCanvas;
    public GameObject WinCanvas;
    public GameObject FailCanvas;

    public MapManager mapManager;

    private bool isEnd=false;

    void Awake()
    {
        if (Instance != null && Instance != this)//检测Instance是否存在且只有一个
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            //Debug.Log("LevelInstance");
        }
    }
    void Start()
    {
        JsonIO.InitGameData();
        JsonIO.InitLevelData(GameManager.Instance.iLevel);
        waves = JsonIO.GetWaves();
        mapType = JsonIO.GetMap();
        levelRequest = JsonIO.GetScoreRequest();
        EnemyGroup = transform.Find("EnemyGroup").gameObject;
        Map = transform.Find("Map").gameObject;
        GenerateTileNode();

        levelPoints = new Points(100, 100, 100);
        finishCreate = false;

        StartCanvas.SetActive(true);
        WinCanvas.SetActive(false);
        FailCanvas.SetActive(false);
        StartCanvas.GetComponent<StartIntroduceUI>().InitText();
        PauseBgUI.Instance.ShowWhiteBg();
        GameManager.Instance.Set1xTimeScale();
        Debug.Log("LevelSpan:" + GetLevelTime());


    }
    public void StartLevel()
    {
        CountDown.GetComponent<CountDownUI>().StartCountDown(6, 1);
        CountDown.position = new Vector2(waves[0].startX + 0.5f, waves[0].startY + 0.5f);
        StartCoroutine(DrawAllRoute(1));
        Invoke("StartDeployEnemy", 6f);
        PauseBgUI.Instance.HideWhiteBg();
        //StartCanvas.SetActive(false);
    }
    private void Update()
    {
        levelPoints.DeployPoints += Time.deltaTime / 5;
    }
    public bool SpendPoints(PointsType pointsType, float points)
    {
        return levelPoints.SpendPoint(pointsType, points);

    }
    public void AddPoints(PointsType pointsType, float points)
    {
        levelPoints.AddPoint(pointsType, points);
    }
    public void ShowWinOrFailCanvas(bool win)
    {
        if (isEnd) return;
        string str = "" + MyTool.PraseRequest(levelRequest[0].scoreType, levelRequest[0].requestNum, levelRequest[0].actualNum) + "\n"
                       + MyTool.PraseRequest(levelRequest[1].scoreType, levelRequest[1].requestNum, levelRequest[1].actualNum) + "\n"
                       + MyTool.PraseRequest(levelRequest[2].scoreType, levelRequest[2].requestNum, levelRequest[2].actualNum);
        if (win)
        {
            WinCanvas.SetActive(true);
            WinCanvas.GetComponent<WinUI>().content.text = str;
            WinCanvas.GetComponent<WinUI>().InitWinUI(levelRequest);
            FailCanvas.SetActive(false);
            SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_success);
            GameManager.Instance.Set0xTimeScale();
            isEnd = true;
        }
        else
        {
            //Debug.Log("LoadFail");
            WinCanvas.SetActive(false);
            FailCanvas.SetActive(true);
            FailCanvas.GetComponent<StartIntroduceUI>().content.text = str;
            SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_fail);
            PlayerPrefs.SetInt("LevelScore" + GameManager.Instance.iLevel, 0);
            GameManager.Instance.Set0xTimeScale();
            isEnd = true;
        }
        //GameManager.Instance.Set0xTimeScale();

    }
    public void OnScoreEvent(ScoreType scoreType, int deltaNum)
    {
        for (int i = 0; i < levelRequest.Length; i++)//对于每一个得分条件
        {
            if (scoreType == levelRequest[i].scoreType)//如果得分事件类型匹配
            {
                switch (scoreType)
                {
                    case ScoreType.EnemyEscapeNum:
                        levelRequest[i].actualNum += deltaNum;
                        break;
                    case ScoreType.CellDeployNum:
                        levelRequest[i].actualNum += deltaNum;
                        break;
                    case ScoreType.EnemyRouteLength:
                        {
                            int num = levelRequest[i].actualNum;
                            levelRequest[i].actualNum = num < deltaNum ? deltaNum : num;
                            break;
                        }
                    case ScoreType.NormalCellSurviveNum:
                        {
                            levelRequest[i].actualNum += deltaNum;
                            //int num = levelRequest[i].actualNum;
                            //levelRequest[i].actualNum = num < deltaNum ? deltaNum : num;
                            break;
                        }
                }

                if (OnScoreChange != null)
                {
                    OnScoreChange(levelRequest);
                }

            }
            else continue;
        }
    }


    public static bool IsOutOfMapEdge(Transform trans)
    {
        float x = trans.position.x;
        float y = trans.position.y;
        if (x < 0 || x > 16 || y < 0 || y > 0) return false;
        else return true;
    } 

    public void StartDeployEnemy()
    {

        StartCoroutine(CreateEnemy());
    }
    private void GenerateTileNode()//初始化寻路节点
    {
        Debug.Log("GenerateTileNode");
        AllNodeGroup = new AStarNode[MapWidth, MapHeight];
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                Vector2 nodePos2 = new Vector2((float)i + 0.5f, (float)j + 0.5f);
                Vector3 nodePos3 = new Vector3((float)i + 0.5f, (float)j + 0.5f, 0);
                GameObject node = Instantiate(Node, nodePos3, Quaternion.identity, Map.transform);
                node.name = "Node(" + i + "," + j + ")";
                node.GetComponent<AStarNode>().InitNode((TileType)mapType[i, j], nodePos2, i, j);
                node.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
                if(mapType[i, j] == 3)
                {
                    GameObject norCell = Instantiate(normalCell, nodePos3, Quaternion.identity, node.transform);
                    norCell.name = "normalCell";
                }
                AllNodeGroup[i, j] = node.GetComponent<AStarNode>();
            }
        }
    }
    public List<AStarNode> GetAroundNodes(AStarNode curNode)//查找节点周围
    {
        //Debug.Log(curNode.name);
        List<AStarNode> retGroup = new List<AStarNode>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0 || i * j != 0)
                {
                    continue;
                }
                int y = curNode.posY + i;
                int x = curNode.posX + j;

                if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                {
                    retGroup.Add(AllNodeGroup[x, y]);
                }
            }
        }
        return retGroup;
    }
    public AStarNode GetNodeByPos(Vector2 pos)//根据位置返回寻路节点
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        if (x <= 0 && y >= 0) return AllNodeGroup[0, y];
        if (x >= 15 && y <= 8&&y>=0) return AllNodeGroup[15, y];
        if (y >= 8 && x >= 0&&x<=15) return AllNodeGroup[x, 8];
        if (y <= 0 && x <= 15) return AllNodeGroup[x, 0];
        if (x >= 0 && y >= 0 && y <= 8 && x <= 15) return AllNodeGroup[x, y];
        return null;

    }
    public AStarNode GetNodeByPos(int x, int y)//根据位置返回寻路节点
    {
        if (x < 0 || x > 15 || y < 0 || y > 8)
        {
            Debug.LogError("PosOutOfRange!(" + x + "," + y + ")");
            throw new System.Exception();
        }
        return AllNodeGroup[x, y];
    }
    private IEnumerator CreateEnemy()
    {

        for (int i = 0; i < waves.Length; i++)
        {
            DrawDefaultRoute(i);
            curWave = i;
            for (int j = 0; j < waves[i].enemyNum; j++)
            {
                //GameObject enemy = Instantiate(enemys[waves[i].enemyType], new Vector3(waves[i].startX+0.5f, waves[i].startY+0.5f,0), Quaternion.identity,EnemyGroup.transform);//随机生成
                GameObject enemy = Instantiate(PrefabManager.GetEnemyPrefab((ActorType)(waves[i].enemyType)), new Vector3(-1, -1, 0), Quaternion.identity, EnemyGroup.transform);//随机生成
                enemy.name = "Enemy" + i + "," + j;
                enemy.GetComponent<EnemyMotion>().startPos = new Vector2(waves[i].startX, waves[i].startY);
                enemy.GetComponent<EnemyMotion>().endPos = new Vector2(waves[i].endX, waves[i].endY);
                enemy.GetComponent<EnemyMotion>().FindPathType = (FindPathType)waves[i].findPathType;
                enemy.GetComponent<EnemyMotion>().originSpeed = waves[i].speed;
                enemy.GetComponentInChildren<EnemyMotion>().enemyType = (EnemyType)(waves[i].enemyType-14);
                enemy.GetComponent<EnemyHealth>().InitHealth *= 1 + ((float)i / 10); 
                enemy.SetActive(true);
                /*enemy.GetComponent<EnemyMotion>().draw = j==0?true:false;*/

                yield return new WaitForSeconds(waves[i].initDuration);
            }
            if (i + 2 < waves.Length)
            {
                CountDown.GetComponent<CountDownUI>().StartCountDown((int)waves[i].nextWaveDuration + 1, i + 2);
                CountDown.position = new Vector2(waves[i + 1].startX + 0.5f, waves[i + 1].startY + 0.5f);

            }
            yield return new WaitForSeconds(waves[i].nextWaveDuration);
        }
        finishCreate = true;
        InvokeRepeating("CheckSuccess", 0f,1f);
    }
    
    public void CreateOneEnemy(ActorType actorType)
    {
        GameObject enemy = Instantiate(PrefabManager.GetEnemyPrefab(actorType), transform.position, Quaternion.identity, EnemyGroup.transform);//随机生成
        enemy.name = "EnemyInitByDeathCell";
        enemy.GetComponent<EnemyMotion>().startPos = new Vector2(waves[0].startX, waves[0].startY);
        enemy.GetComponent<EnemyMotion>().endPos = new Vector2(waves[0].endX, waves[0].endY);
        enemy.GetComponent<EnemyMotion>().FindPathType = (FindPathType)waves[0].findPathType;
    }

    public void DrawDefaultRoute(int wave)
    {
        Vector4[] vectors = JsonIO.GetDefaultPath();
        AStarAgent agent = transform.GetComponent<AStarAgent>();

        bool issuccess;
        agent.FindPathWithStartAndEndPos(new Vector2(vectors[wave].x, vectors[wave].y), new Vector2(vectors[wave].z, vectors[wave].w), FindPathType.Defult, out issuccess);

        if (issuccess)
        {
            agent.SetPath(new Vector2(vectors[wave].x, vectors[wave].y), new Vector2(vectors[wave].z, vectors[wave].w));
            StartCoroutine(DrawRoute.Drawarrow(drawRoute, agent.wayPointList, 0.05f));
            OnScoreEvent(ScoreType.EnemyRouteLength, agent.wayPointList.Count);
        }
    }

    public IEnumerator DrawAllRoute(float time)
    {
        yield return new WaitForSeconds(time);
        Vector4[] vectors = JsonIO.GetDefaultPath();
        List<Vector4> vector4s = new List<Vector4>();

        for (int i = 0; i < vectors.Length; i++)
        {
            if (!vector4s.Contains(vectors[i]))
            {
                vector4s.Add(vectors[i]);
                DrawDefaultRoute(i);
                yield return new WaitForSeconds(1);
            }
            
        }
    }

    public void CheckSuccess()
    {
        if (!finishCreate || EnemyManager.Instance.GetEnemyNum() > 0|| levelRequest[0].requestNum < levelRequest[0].actualNum) return;
        ShowWinOrFailCanvas(true);


    }
    public void CheckFail()
    {
        if (levelRequest[0].requestNum < levelRequest[0].actualNum)
        {
            ShowWinOrFailCanvas(false);
        }
    }
    public float GetLevelTime() 
    {
        Wave[] waves = JsonIO.GetWaves();
        float time = 0;

        for(int i = 0; i < waves.Length; i++)
        {
            time += waves[i].initDuration * waves[i].enemyNum + waves[i].nextWaveDuration;
        }
        return time;
    }

}
