using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTool : MonoBehaviour
{
    /// <summary>
    /// 对物体进行旋转
    /// </summary>/// 
    /// <param name="_transform"></param>需要旋转的物体
    /// <param name="lockwise"></param>是否是顺时针（反之逆时针）
    /// <param name="speed"></param>旋转角速度
    /// <param name="howlong"></param>多长时间内完成
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
    /// <summary>
    /// 对物体进行缩放
    /// </summary>/// 
    /// <param name="_transform"></param>需要缩放的物体
    /// <param name="targetScale"></param>目标缩放大小
    /// <param name="howlong"></param>多长时间内完成
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
    public static string PraseRequest(ScoreType scoreType, int num)
    {
        string str;

        switch (scoreType)
        {
            case ScoreType.EnemyEscapeNum:
                str = "逃脱的病原体不超过" + num.ToString() + "个(0/" + num.ToString() + ")";
                break;
            case ScoreType.CellDeployNum:
                str = "放置免疫细胞不超过" + num.ToString() + "个(0/" + num.ToString() + ")";
                break;
            case ScoreType.EnemyRouteLength:
                str = "病原体行进路径达到" + num.ToString() + "格(0/" + num.ToString() + ")";
                break;
            case ScoreType.NormalCellSurviveNum:
                str = "健康细胞死亡不超过" + num.ToString() + "个(0 /" + num.ToString() + ")";
                break;
            default:
                str = "";
                break;
        }
        return str;
    }
    public static string PraseRequest(ScoreType scoreType, int num,int actualNum)
    {
        string str;

        switch (scoreType)
        {
            case ScoreType.EnemyEscapeNum:
                str = "逃脱的病原体不超过" + num.ToString() + "个("+ actualNum.ToString() + "/" + num.ToString() + ")";
                break;
            case ScoreType.CellDeployNum:
                str = "放置免疫细胞不超过" + num.ToString() + "个("+ actualNum.ToString() + "/" + num.ToString() + ")";
                break;
            case ScoreType.EnemyRouteLength:
                str = "病原体行进路径达到" + num.ToString() + "格("+ actualNum.ToString() + "/" + num.ToString() + ")";
                break;
            case ScoreType.NormalCellSurviveNum:
                str = "健康细胞死亡不超过" + num.ToString() + "个(" + actualNum.ToString() + "/" + num.ToString() + ")";
                break;
            default:
                str = "";
                break;
        }
        return str;
    }
    public static void DoFade(GameObject gameObject)
    {
        //gameObject.GetComponent<SpriteRenderer>();
    }
    public IEnumerator DoFade(SpriteRenderer sp, float alpha,float time)
    {
        float dur = 0;
        float startAlpha = sp.color.a;
        while (dur < time)
        {
            dur += Time.deltaTime;
            alpha = startAlpha > alpha?Mathf.Lerp(alpha, startAlpha,dur/time): Mathf.Lerp(startAlpha, alpha, dur / time);
            sp.color = new Color(sp.color.r,
                                    sp.color.g,
                                    sp.color.b,
                                    alpha);
            yield return new WaitForFixedUpdate();
        }
         
    }
    public static IEnumerator DoMoveY(float y,Transform trans,float time)
    {
        while (!Mathf.Approximately(y, trans.position.y))
        {
            float deltaY = (trans.position.y - y) / time;
            trans.position += new Vector3(0, deltaY*0.0166667f, 0);
            yield return new WaitForFixedUpdate();
        }
        
    }
}
