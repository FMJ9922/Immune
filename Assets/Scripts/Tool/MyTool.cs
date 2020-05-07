using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTool : MonoBehaviour
{
   
    /*public static void DoRotate(Transform _transform,Vector3 angle,float speed,float howlong)
    {
        IEnumerator enumerator = RotateEnumerator(_transform, angle, speed, howlong);
        Coroutine coroutine = StartCoroutine(enumerator);
    }*/
    public static IEnumerator DoRotate(Transform _transform, bool clockwise, float speed, float howlong)
    {
        float timer = howlong;
        Vector3 angle = clockwise ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
        while (timer>=0)
        {
            timer -= Time.deltaTime;
            _transform.Rotate(angle * speed * Time.deltaTime);
            yield return 0;
        }
    }
    public static IEnumerator DoScale(Transform _transform, float targetScale, float howlong)
    {
        float timer = howlong;
        Vector3 originScale = _transform.localScale;
        float deltaScale = targetScale - originScale.x;
        float speed = deltaScale / howlong;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            //Debug.Log(timer);
            _transform.localScale += speed * Vector3.one * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// 根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t"></param>T值
    /// <param name="p0"></param>起始点
    /// <param name="p1"></param>控制点
    /// <param name="p2"></param>目标点
    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
    private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    /// <summary>
    /// 获取存储贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint"></param>控制点
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector3[] GetBeizerList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateCubicBezierPoint(t, startPoint,
                controlPoint, endPoint);
            path[i - 1] = pixel;
            //Debug.Log(path[i - 1]);
        }
        return path;

    }
}
