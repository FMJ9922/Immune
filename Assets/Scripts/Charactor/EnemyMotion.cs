using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion : MonoBehaviour
{
    public float speed = 5;//设置敌人的速度
    private WayPoint[] p;//定义数组
    private int index = 0;//坐标点

    void Start()
    {
        p = JsonIO.GetWayPoints();//调用Waypoint脚本获取节点的位置信息
    }

    void Update()
    {
        Move();//每一帧执行方法
    }

    void Move()
    {
        transform.Translate((p[index].position - transform.position).normalized * Time.deltaTime * speed);//移动，节点到当前位置的向量差的单位差*完成上一帧的时间*速度
        if (Vector3.Distance(p[index].position, transform.position) < 0.03f)//三维坐标，距离（节点，当前位置）小于0.2f的时候执行
        {
            index++;//增加索引，也就获取到下个节点坐标
            if (index > p.Length - 1)//如果大于最后一个节点时执行
            {
                Destroy(this.gameObject);//销毁物体
            }
        }
    }
}
