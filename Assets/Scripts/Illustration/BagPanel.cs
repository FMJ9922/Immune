using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : ViewBase
{
    public ArticleType articleType;
    public int  num;
    int a = 0;

    #region 数据

    private List<Article> articles = new List<Article>();
   // private List<GameObject> articleItems = new List<GameObject>();
    #endregion


    public GameObject articleItemPrefab;
    public GameObject ImagePrefab;
    private BagGrid[] bagGrids;
    public Information Information;
    public static BagPanel _instance;

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
        articles.Add(new Article("eb", "EnemyCell/eb", ArticleType.Enemy, "属性"));
        articles.Add(new Article("病毒宿主", "EnemyCell/病毒宿主", ArticleType.Enemy, "属性"));
        articles.Add(new Article("黄金葡萄球", "EnemyCell/黄金葡萄球", ArticleType.Enemy, "属性"));
        articles.Add(new Article("巨细胞病毒", "EnemyCell/巨细胞病毒", ArticleType.Enemy, "属性"));
        articles.Add(new Article("梅毒螺旋体", "EnemyCell/梅毒螺旋体", ArticleType.Enemy, "属性"));
        articles.Add(new Article("天花病毒", "EnemyCell/天花病毒", ArticleType.Enemy, "属性"));
        // 正常细胞
       // articles.Add(new Article("红细胞", "Sprite/book1", ArticleType.Cell, "属性"));
       // articles.Add(new Article("其他正常细胞", "Sprite/book2", ArticleType.Cell, "属性"));

    }

    // 加载数据 ( 加载全部的数据 )
    public void LoadData()
    {

        for (int i = 0; i < articles.Count; i++)
        {
            //HideAllArticleItems();
            if (articles[i].articleType == articleType)
            {
                GameObject obj = GameObject.Instantiate(articleItemPrefab);//实化物体
                ArticleItem articleItem = obj.GetComponent<ArticleItem>();//设置给物品
                articleItem.SetArticle(articles[i]);
                if (a == num)
                {
                    a = 0;
                }
                if (a < num)
                {  
                    bagGrids[a].SetArticleItem(articleItem);//设置给格子 
                    a++;
                }
                       
            }
               
        }
    }

    
}


       
   
