using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagGrid : MonoBehaviour
{

    private  ArticleItem articleItem;
    

    // 设置格子数据
    public void SetArticleItem(ArticleItem articleItem)
    {
        this.articleItem = articleItem;
        this.articleItem.gameObject.SetActive(true);
        //设置父物体
        this.articleItem.transform.SetParent(transform);
       // 设置位置
        this.articleItem.transform.localPosition = Vector3.zero;
        // 设置Scale
        this.articleItem.transform.localScale = Vector3.one;
    }

    public void IsOn()
    {
        BagPanel._instance.Information.Show();
        BagPanel._instance.Information.SetShowInfo(this.articleItem.GetArticleInfo());
    }

}


    