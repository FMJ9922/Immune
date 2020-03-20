using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public static Transform[] p;//定义数组
    private void Awake()//在脚本执行时执行
    {
        //获取每个数组的参数，也就是节点位置
        p = new Transform[transform.childCount];
        for (int i = 0; i < p.Length; i++)
        {
            p[i] = transform.GetChild(i);
        }
    }
}
