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
    public string name;
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
        string path = "";
        if ((int)actorType < 15)
        {
            path = "Cell";
            image.sprite = Resources.Load<Sprite>(path + "/" + ((int)actorType).ToString() + actorType.ToString() + "/" + actorType.ToString());
            name = JsonIO.GetCellData((CellType)(int)actorType).introduce;
            Debug.Log(name);
        }
        else if ((int)actorType < 20) path = "Enemy";

        

    }
}

