using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawRoute : MonoBehaviour
{
    private static GameObject arrow;

    public GameObject Arrow;
    private void Awake()
    {
        arrow = Arrow;
    }


    public static IEnumerator Drawarrow(Transform parentNode,List<Vector3> vector3s,float duration)
    {
        //Debug.Log("???");
        if (vector3s.Count == 0) yield break;
        GameObject[] gameObjects = new GameObject[2*vector3s.Count - 2];
        int i = 0;
        int n = 0;
        do
        {
            if(i>0&&i< vector3s.Count - 1)
            {
                Vector3 position0 = vector3s[i]*0.8f+ vector3s[i + 1]*0.1f + vector3s[i - 1] * 0.1f;
                Quaternion quaternion0 = Quaternion.LookRotation(vector3s[i + 1] - vector3s[i-1], new Vector3(0, 0, 1));
                GameObject newarrow0 = Instantiate(arrow, position0, quaternion0, parentNode);
                newarrow0.name = "Arrow(" + n + ")";
                Color color0 = newarrow0.GetComponent<MeshRenderer>().material.color;
                newarrow0.GetComponent<MeshRenderer>().material.color = new Color(color0.r, color0.g, color0.b, 0);
                Fade(newarrow0, 1f, 0.5f);
                gameObjects[n] = newarrow0;
                n++;
            }
            Vector3 position = (vector3s[i] + vector3s[i + 1]) / 2;
            Quaternion quaternion = Quaternion.LookRotation(vector3s[i + 1] - vector3s[i], new Vector3(0, 0, 1));
            GameObject newarrow = Instantiate(arrow, position, quaternion, parentNode);
            newarrow.name = "Arrow(" + i + ")";
            Color color = newarrow.GetComponent<MeshRenderer>().material.color;
            newarrow.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0);
            Fade(newarrow, 1f, 0.5f);
            gameObjects[n] = newarrow;
            n++;
            i++;
            yield return new WaitForSeconds(duration);
        }
        while (i != vector3s.Count - 1);
        yield return new WaitForSeconds(1f);
        for (int j = 0; j< 2 * vector3s.Count - 3; j++)
        {
            Fade(gameObjects[j], 0, 0.5f);
            yield return new WaitForSeconds(duration);
        }
        yield return new WaitForSeconds(2f);
        for (int k = 0; k < 2*vector3s.Count - 2; k++)
        {
            Destroy(gameObjects[k]);
        }
    }
    public static void Fade(GameObject obj,float endValue, float duration)
    {
        Material mat = obj.GetComponent<MeshRenderer>().material;
        mat.DOFade(endValue, duration);
    }
}
