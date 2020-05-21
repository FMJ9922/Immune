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

    //private Article[] articles = new Article[21];
   // private List<GameObject> articleItems = new List<GameObject>();
    #endregion

    public GameObject articleItemPrefab;
    //public GameObject ImagePrefab;
    private BagGrid[] bagGrids;
    public Information Information;
    //public static BagPanel _instance;

    #region Unity回调

    delegate void ActorTypeName(int actortype);
    

    private void Awake()
    {
       
        //InitArticleData();
        bagGrids = transform.GetComponentsInChildren<BagGrid>();
        
    }

    private void Start()
    {
        LoadData();
        ActiveButton();
    }



    #endregion


    // 初始化物品数据
    public void InitArticleData()
    {
       /*
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
       // articles.Add(new Article("其他正常细胞", "Sprite/book2", ArticleType.Cell, "属性"));*/

    }

    public ArticleType PraseEnum(int actorType)
    {
        ArticleType articleType;
        switch (actorType)
        {
            //近战
            case 0:
                articleType = ArticleType.SRCell;
                return articleType;
            case 3:
                articleType = ArticleType.SRCell;
                return articleType;
            case 7:
                articleType = ArticleType.SRCell;
                return articleType;
            //远程
            case 1:
                articleType = ArticleType.LRCell;
                return articleType;
            case 13:
                articleType = ArticleType.LRCell;
                return articleType;
            case 5:
                articleType = ArticleType.LRCell;
                return articleType;
            //辅助
            case 2:
                articleType = ArticleType.HelpCell;
                return articleType;
            case 4:
                articleType = ArticleType.HelpCell;
                return articleType;
            case 6:
                articleType = ArticleType.HelpCell;
                return articleType;
            case 8:
                articleType = ArticleType.HelpCell;
                return articleType;
            case 9:
                articleType = ArticleType.HelpCell;
                return articleType;
            case 12:
                articleType = ArticleType.HelpCell;
                return articleType;
            //病毒细胞
            case 14:
                articleType = ArticleType.Enemy;
                return articleType;
            case 15:
                articleType = ArticleType.Enemy;
                return articleType;
            case 16:
                articleType = ArticleType.Enemy;
                return articleType;
            case 17:
                articleType = ArticleType.Enemy;
                return articleType;
            case 18:
                articleType = ArticleType.Enemy;
                return articleType;
            case 19:
                articleType = ArticleType.Enemy;
                return articleType;
            //正常细胞
            case 20:
                articleType = ArticleType.Cell;
                return articleType;
            case 21:
                articleType = ArticleType.Cell;
                return articleType;
        }
        return articleType = ArticleType.Cell; 
        
    }
    // 加载数据 ( 加载全部的数据 )
    public void LoadData()
    {
        

        for (int i = 0; i < 21; i++)
        {
            
            if (PraseEnum(i) == articleType)
            {
                GameObject obj = Instantiate(articleItemPrefab,transform);//实化物体
                obj.name = i.ToString();
                if (a == num)
                {
                    a = 0;
                }
                if (a < num)
                {  
                    bagGrids[a].SetArticleItem(obj.transform);//设置给格子 
                    a++;
                }
                string path ="";
                if (i < 15) path = "Cell";
                else if (i < 20) path = "Enemy";

                //Debug.Log(obj.GetComponent<SpriteRenderer>()==null);
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>(path + "/" + i.ToString() + ((ActorType)i).ToString() + "/" + ((ActorType)i).ToString());
                
            }
               
        }
    }
    public void ActiveButton()
    {
        ActorTypeName OnClick = new ActorTypeName(SetInformation);
        OnClick += SetInformation;
        for(int i= 0; i < gameObject.GetComponentsInChildren<Button>().Length; i++)
        {
            Button btn = gameObject.GetComponentsInChildren<Button>()[i];
            //Debug.Log(int.Parse(btn.gameObject.name));
            btn.onClick.AddListener(() => { OnClick(int.Parse(btn.gameObject.name)); });
        }

    }
    public void SetInformation(int actorType)
    {
        //Debug.Log("actor" + actorType);
        Information.SetActorType(actorType);
    }
    
}


       
   
