using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



//信息

public enum ArticleType {
    //类型
    SRCell,
    LRCell,
    HelpCell,
    Enemy,
    Cell
}

public class Article
{
    public string name;//名字
    public string spritePath;//图片（根据路径加载图片）
    public ArticleType articleType;//枚举类型
    public string attribute;//属性



    public Article(string name,string spritePath,ArticleType articleType,string attribute)
    {
        this.name = name;
        this.attribute = attribute;
        this.spritePath = spritePath;
        this.articleType = articleType;
        
    }


    //TMP的对应代码
    /*public virtual string GetArticleInfo()
    {
        StringBuilder stringBuilder = new StringBuilder();
       // stringBuilder.Append("<color=#92FF26>");
        stringBuilder.Append("名称:").Append(this.name);
       // stringBuilder.Append("</color>");
        stringBuilder.Append("\n");
              
        stringBuilder.Append("类型:").Append(GetTypeName(this.articleType)).Append("\n");
      
        stringBuilder.Append("属性:").Append(this.attribute);

        return stringBuilder.ToString();
    }

    public string GetTypeName(ArticleType articleType)
    {
        switch (articleType)
        {
            case ArticleType.SRCell:
                return "近战";
            case ArticleType.LRCell:
                return "远程";
            case ArticleType.HelpCell:
                return "辅助";
            case ArticleType.Enemy:
                return "病毒";
            case ArticleType.Cell:
                return "正常细胞";
        }
        return "";
    }*/



}
