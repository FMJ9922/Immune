using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } = null;
    public GameObject[] enemys;
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
    public float DeployPoints { get; private set; }

    

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
        waves = JsonIO.GetWaves();
        mapType = JsonIO.GetMap();
        EnemyGroup = transform.Find("EnemyGroup").gameObject;
        Map = transform.Find("Map").gameObject;
        GenerateTileNode();
        StartCoroutine(CreateEnemy());
        DeployPoints = 100;
    }
    private void Update()
    {
        DeployPoints += Time.deltaTime/5;
    }
    public bool SpendPoints(float points)
    {
        if (points <= DeployPoints)
        {
            DeployPoints -= points;
            return true;
        }
        else
        {
            return false;
        }
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

                AllNodeGroup[i, j] = node.GetComponent<AStarNode>();
            }
        }
    }
    public List<AStarNode> GetAroundNodes(AStarNode curNode)//查找节点周围
    {
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
        if (x < 0 && y >= 0) return AllNodeGroup[0, y];
        else if (x > 15 && y <= 8) return AllNodeGroup[15, y];
        else if (y > 8 && x >= 0) return AllNodeGroup[x, 8];
        else if (y < 0 &&x <= 15) return AllNodeGroup[x, 0];
        else return AllNodeGroup[x, y];
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
        DrawDefaultRoute(0);
        for (int i = 0; i < waves.Length; i++)
        {
            curWave = i;
            for (int j = 0; j < waves[i].enemyNum; j++)
            {
                //GameObject enemy = Instantiate(enemys[waves[i].enemyType], new Vector3(waves[i].startX+0.5f, waves[i].startY+0.5f,0), Quaternion.identity,EnemyGroup.transform);//随机生成
                GameObject enemy = Instantiate(enemys[waves[i].enemyType], new Vector3(-1, -1, 0), Quaternion.identity, EnemyGroup.transform);//随机生成
                enemy.name = "Enemy" + i + "," + j;
                enemy.GetComponent<EnemyMotion>().startPos = new Vector2(waves[i].startX, waves[i].startY);
                enemy.GetComponent<EnemyMotion>().endPos = new Vector2(waves[i].endX, waves[i].endY);
                enemy.GetComponent<EnemyMotion>().FindPathType = (FindPathType)waves[i].findPathType;
                enemy.GetComponent<EnemyMotion>().speed = waves[i].speed;
                enemy.GetComponentInChildren<EnemyAnimator>().enemyType = (EnemyType)waves[i].enemyType;

                /*enemy.GetComponent<EnemyMotion>().draw = j==0?true:false;*/

                yield return new WaitForSeconds(waves[i].initDuration);
            }

            yield return new WaitForSeconds(waves[i].nextWaveDuration);
        }
    }
    public void CreateOneEnemy()
    {
        GameObject enemy = Instantiate(enemys[waves[0].enemyType], transform.position, Quaternion.identity, EnemyGroup.transform);//随机生成
        enemy.name = "Enemy" + 0 + "," + 0;
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
        }
    }
}
