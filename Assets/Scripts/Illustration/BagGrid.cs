using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagGrid : MonoBehaviour
{

    //private  ArticleItem articleItem;
    

    // 设置格子数据
    public void SetArticleItem(Transform trans)
    {
        trans.gameObject.SetActive(true);
        //设置父物体
        trans.transform.SetParent(transform);
        // 设置位置
        trans.transform.localPosition = Vector3.zero;
        // 设置Scale
        trans.transform.localScale = Vector3.one;
    }


}


    