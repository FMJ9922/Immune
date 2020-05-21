using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Information :MonoBehaviour
{
    private ActorType actorType;

    public Image image;
   // public GameObject ImagePrefab;
    public Text Name;//名字
    public Text Attribute;//属性
    private void Start()
    {
    }

    public void SetActorType(int actorType)
    {
        this.actorType = (ActorType)actorType;
        InitData();
       
    }

    private void InitData()
    {
       // GameObject image = Instantiate(ImagePrefab, transform);
        string path = "";
        if ((int)actorType < 15)
        {
            path = "Cell";
          //  image.GetComponent<Image>().sprite = Resources.Load<Sprite>(path + "/" + ((int)actorType).ToString() + actorType.ToString() + "/" + actorType.ToString());
        image.sprite = Resources.Load<Sprite>(path + "/" + ((int)actorType).ToString() + actorType.ToString() + "/" + actorType.ToString());
            Name.text = JsonIO.GetCellData((CellType)(int)actorType).name.ToString();
            //Debug.Log(Name);
            Attribute.text = "花费：" + JsonIO.GetCellData((CellType)(int)actorType).initCost.ToString() +
                "\n" + "攻击范围：" + JsonIO.GetCellData((CellType)(int)actorType).atkRange.ToString() +
                  "\n" + "攻击伤害：" + JsonIO.GetCellData((CellType)(int)actorType).atkDamage.ToString() +
                  "\n\n" + "介绍：\n" + JsonIO.GetCellData((CellType)(int)actorType).introduce.ToString();

        }
        else if ((int)actorType < 20) path = "Enemy";

        

    }
}

