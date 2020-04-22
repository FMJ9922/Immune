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
        //Debug.Log(vector3s.Count);
        if (vector3s.Count == 0) yield break;
        GameObject[] gameObjects = new GameObject[vector3s.Count - 1];
        int i = 0;
        do
        {
            Vector3 position = (vector3s[i] + vector3s[i + 1]) / 2;
            Quaternion quaternion = Quaternion.LookRotation(vector3s[i + 1] - vector3s[i], new Vector3(0, 0, 1));

            GameObject newarrow = Instantiate(arrow, position, quaternion, parentNode);
            newarrow.name = "Arrow(" + i + ")";
            Color color = newarrow.GetComponent<MeshRenderer>().material.color;
            newarrow.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0);
            Fade(newarrow, 1f, 0.5f);
            gameObjects[i] = newarrow;
            i++;
            yield return new WaitForSeconds(duration);
        }
        while (i != vector3s.Count - 1);

        for(int j = 0; j< vector3s.Count - 1; j++)
        {
            Fade(gameObjects[j], 0, 0.5f);
            yield return new WaitForSeconds(duration);
        }
        yield return new WaitForSeconds(0.5f);
        for (int k = 0; k < vector3s.Count - 1; k++)
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
