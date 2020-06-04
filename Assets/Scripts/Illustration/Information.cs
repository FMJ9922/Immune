using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Information : MonoBehaviour
{
    public static Information Instance { get; private set; } = null;
    void Awake()
    {
        if (Instance != null && Instance != this)//检测Instance是否存在且只有一个
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private ActorType actorType;

    // public Image image;

    public GameObject ImagePrefab;
    public Image ImageCell;
    /*private TMP_Text Name;
    private TMP_Text Data;
    private TMP_Text Type;
    private TMP_Text Introduce;*/
    //  public Text Name;//名字
    // public Text Attribute;//属性

    public TMP_Text Name;
    public TMP_Text Data;
    public TMP_Text Type;
    public TMP_Text Introduce;
    private void Start()
    {
        /*  Name = transform.Find("Name").GetComponent<TMP_Text>();
          Data = transform.Find("Data").GetComponent<TMP_Text>();
          Data = transform.Find("Type").GetComponent<TMP_Text>();
          Introduce = transform.Find("Introduce").GetComponent<TMP_Text>();*/
    }

    public void SetActorType(int actorType)
    {
        this.actorType = (ActorType)actorType;
        InitData();


    }

    private void InitData()
    {
      //  GameObject image = Instantiate(ImagePrefab, transform);
        string path = "";
        if ((int)actorType < 15)
        {
            path = "Cell";
           
            //image.GetComponent<Image>().sprite = Resources.Load<Sprite>(path + "/" + ((int)actorType).ToString() + actorType.ToString() + "/" + actorType.ToString()); ;
            ImageCell.GetComponent<Image>().sprite = Resources.Load<Sprite>(path + "/" + ((int)actorType).ToString() + actorType.ToString() + "/" + actorType.ToString());
            
            Name.text = JsonIO.GetCellData((CellType)(int)actorType).name.ToString();
            this.Type.text = JsonIO.GetCellData((CellType)(int)actorType).type;
            Data.text = "伤害:" + JsonIO.GetCellData((CellType)(int)actorType).atkDamage.ToString() + "   " +
                        "范围:" + JsonIO.GetCellData((CellType)(int)actorType).atkRange.ToString() + "\n" +
                        "冷却:" + JsonIO.GetCellData((CellType)(int)actorType).atkDuration.ToString() + "秒" + "   " +
                        "花费:" + JsonIO.GetCellData((CellType)(int)actorType).initCost.ToString() + "\n" +
                        "技能:" + JsonIO.GetCellData((CellType)(int)actorType).ability.ToString();
            Introduce.text = "简介:" + JsonIO.GetCellData((CellType)(int)actorType).introduce;

        }
        else if ((int)actorType < 20) path = "Enemy";
    }

    /* public void dateInfo(CellType cellType)
     {
         Name.text = JsonIO.GetCellData(cellType).name;
         Data.text = "伤害:" + JsonIO.GetCellData(cellType).atkDamage.ToString() + "/n" +
                     "冷却:" + JsonIO.GetCellData(cellType).atkDuration.ToString() + "秒" + "/n" +
                     "范围:" + JsonIO.GetCellData(cellType).atkRange.ToString() + "/n" +
                     "花费:" + JsonIO.GetCellData(cellType).initCost.ToString() + "/n" +
                     "技能:" + JsonIO.GetCellData(cellType).ability;
         this.Type.text = "定位:" + JsonIO.GetCellData(cellType).type;
         Introduce.text = "简介:" + JsonIO.GetCellData(cellType).introduce;
     }*/
}

