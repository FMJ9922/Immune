using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleItem : MonoBehaviour
{
   
    private Image articleSprite;//加载图片
   

    private Article article;//物品数据


      private void Awake()
    {
        this.article = article;
        articleSprite = transform.GetComponent<Image>();//图片
      

    }



 
    public void SetArticle( Article article )
    {
        this.article = article;

        // 显示数据
        articleSprite.sprite = Resources.Load<Sprite>( article.spritePath);
       

    }

   
}