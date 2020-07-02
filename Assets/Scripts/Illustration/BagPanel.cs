﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : ViewBase
{
    public ArticleType articleType;
    public int  num;
    int a = 0;
    public CellType cellType;

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
        SetInformation(0);
    }



    #endregion


    public static ArticleType PraseEnum(int actorType)
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
            case 11:
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
            case 10:
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
            case 13:
                articleType = ArticleType.Cell;
                return articleType;
            case 20:
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
                if (i < 14) path = "Cell";
                else if (i>13&&i < 20) path = "Enemy";
                else if (i > 19 && i < 23) path = "Cell";

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
            btn.onClick.AddListener(() => 
            { 
                OnClick(int.Parse(btn.gameObject.name));
                SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_page_turn); 
            });
        }

    }
    public void SetInformation(int actorType)
    {
        //Debug.Log("actor" + actorType);
         Information.SetActorType(actorType);
       // Information.Instance.dateInfo(cellType);
    }
    
}


       
   
