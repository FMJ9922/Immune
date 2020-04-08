using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion : MonoBehaviour
{
    public float speed = 1;//设置敌人的速度
    //private WayPoint[] p;//定义数组
    private List<Vector3> wayPointList;
    private int index = 0;//坐标点
    public Vector2 startPos;
    public Vector2 endPos;
    private AStarAgent agent;
    public FindPathType FindPathType;

    void Start()
    {
        speed = 0.5f;
        agent = transform.GetComponent<AStarAgent>();
        InitWayPoint();
        ControlManager.OnPlantCell += ChangeWayPoint;
    }

    private void OnDestroy()
    {
        ControlManager.OnPlantCell -= ChangeWayPoint;
    }
    void InitWayPoint()
    {
        agent.FindPathWithStartAndEndPos(startPos, endPos, FindPathType, out bool success);
        if (success)
        {
            agent.SetPath(startPos, endPos);
            wayPointList = agent.wayPointList;
        }
        
        
    }
    void ChangeWayPoint()
    {
        //Debug.Log("change");
        agent.FindPathWithStartAndEndPos(new Vector2(transform.position.x, transform.position.y), endPos, FindPathType, out bool success);
        if (success)
        {
            //Debug.Log("success");
            agent.SetPath(transform.position, endPos);
            wayPointList = agent.wayPointList;
            index = 0;
        }
        else
        {
            ControlManager.Instance.PathAvaliable = false;
        }

    }

    public EnemyMotion(Vector2 _startPos, Vector2 _endPos, FindPathType _FindPathType)
    {
        this.startPos = _startPos;
        this.endPos = _endPos;
        this.FindPathType = _FindPathType;
    }

    void Update()
    {
        if (transform.GetComponent<EnemyHealth>().Hp > 0)
        {
            Move(wayPointList);
        }
        //每一帧执行方法
    }

    void Move(List<Vector3> wayPointList)
    {   if(wayPointList.Count == 0)
        {
            Destroy(gameObject);
        }
        if (index >= wayPointList.Count) return;
        transform.Translate((wayPointList[index] - transform.position).normalized * Time.deltaTime * speed);//移动，节点到当前位置的向量差的单位差*完成上一帧的时间*速度
        if (Vector3.Distance(wayPointList[index], transform.position) < 0.03f)//三维坐标，距离（节点，当前位置）小于0.2f的时候执行
        {
            index++;//增加索引，也就获取到下个节点坐标
            if (index > wayPointList.Count - 1)//如果大于最后一个节点时执行
            {
                Destroy(this.gameObject);//销毁物体
            }
        }
    }
}
