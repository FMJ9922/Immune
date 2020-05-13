using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : ViewBase
{
    #region 数据

    private List<Article> articles = new List<Article>();

    #endregion


    public GameObject articleItemPrefab;
    private BagGrid[] bagGrids;


    #region Unity回调

    private void Awake()
    {
       
        InitArticleData();
        bagGrids = transform.GetComponentsInChildren<BagGrid>();
    }

    private void Start()
    {
        LoadData();
    }



    #endregion


    // 初始化物品数据
    public void InitArticleData()
    {

        // 近战
        articles.Add(new Article("嗜中性粒细胞", "SRCell/嗜中性粒细胞", ArticleType.SRCell,"属性"));
        articles.Add(new Article("巨嗜细胞", "SRCell/巨嗜细胞", ArticleType.SRCell, "属性"));
        articles.Add(new Article("B淋巴细胞", "SRCell/B淋巴细胞", ArticleType.SRCell, "属性"));
        articles.Add(new Article("效应T细胞", "SRCell/效应T细胞", ArticleType.SRCell, "属性"));
        // 远程
        articles.Add(new Article("嗜酸性粒细胞", "LRCell/嗜酸性粒细胞", ArticleType.LRCell, "属性"));
        articles.Add(new Article("浆细胞", "LRCell/浆细胞", ArticleType.LRCell, "属性"));
        articles.Add(new Article("NK自然杀伤细胞", "LRCell/NK自然杀伤细胞", ArticleType.LRCell, "属性"));
        articles.Add(new Article("调节性T细胞", "LRCell/调节性T细胞", ArticleType.LRCell, "属性"));
        // 辅助
        articles.Add(new Article("嗜碱性粒细胞", "HelpCell/嗜碱性粒细胞", ArticleType.HelpCell, "属性"));
        articles.Add(new Article("树突细胞", "HelpCell/树突细胞", ArticleType.HelpCell, "属性"));
        articles.Add(new Article("T淋巴细胞", "HelpCell/T淋巴细胞", ArticleType.HelpCell, "属性"));
        articles.Add(new Article("辅助性T细胞", "HelpCell/辅助性T细胞", ArticleType.HelpCell, "属性"));
        articles.Add(new Article("记忆B细胞", "HelpCell/记忆B细胞", ArticleType.HelpCell, "属性"));
        // 病毒
        articles.Add(new Article("eb", "Enemy/eb", ArticleType.Enemy, "属性"));
        articles.Add(new Article("病毒宿主", "Enemy/病毒宿主", ArticleType.Enemy, "属性"));
        articles.Add(new Article("黄金葡萄球", "Enemy/黄金葡萄球", ArticleType.Enemy, "属性"));
        articles.Add(new Article("巨细胞病毒", "Enemy/巨细胞病毒", ArticleType.Enemy, "属性"));
        articles.Add(new Article("梅毒螺旋体", "Enemy/梅毒螺旋体", ArticleType.Enemy, "属性"));
        articles.Add(new Article("天花病毒", "Enemy/天花病毒", ArticleType.Enemy, "属性"));
        // 正常细胞
       // articles.Add(new Article("红细胞", "Sprite/book1", ArticleType.Cell, "属性"));
       // articles.Add(new Article("其他正常细胞", "Sprite/book2", ArticleType.Cell, "属性"));

    }

    // 加载数据 ( 加载全部的数据 )
    public void LoadData()
    {

       
        for (int i = 0; i < articles.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(articleItemPrefab);
            ArticleItem articleItem = obj.GetComponent<ArticleItem>();
            articleItem.SetArticle(articles[i]);
            bagGrids[i].SetArticleItem(articleItem);
        }
    }

   

}