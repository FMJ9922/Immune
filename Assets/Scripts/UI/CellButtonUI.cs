using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellButtonUI : MonoBehaviour
{
    public GameObject cellButtonPrefeb;
    int[] cellTypes;
    int num;
    public Sprite bushu;
    public Sprite kangyuan;
    public Sprite linba;

    private static CellButtonUI instance;
    public static CellButtonUI Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
        InitButtonSlot();
        GetComponent<VerticalLayoutGroup>().spacing = 50f;
    }
    public void InitOneButton(int i)
    {
        GameObject cellButton = Instantiate(cellButtonPrefeb, transform);
        cellButton.name = ((CellType)cellTypes[i]).ToString();
        cellButton.GetComponentInChildren<CellButtonInfo>().cellType = (CellType)cellTypes[i];
        string path = "Cell/" + cellTypes[i] + ((CellType)cellTypes[i]).ToString() + "/" + ((CellType)cellTypes[i]).ToString();
        cellButton.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        if (cellTypes[i] < 7 || cellTypes[i] == 10)
        {
            cellButton.transform.GetChild(2).GetComponent<Image>().sprite = bushu;
        }
        else if (cellTypes[i] < 10)
        {
            cellButton.transform.GetChild(2).GetComponent<Image>().sprite = kangyuan;
        }
        else
        {
            cellButton.transform.GetChild(2).GetComponent<Image>().sprite = linba;
        }
    }
    private void InitButtonSlot()
    {
        cellTypes = JsonIO.GetAllowCellType();
        num = cellTypes.Length;
        //Debug.Log(num);
        for (int i = 0; i <num; i++)
        {
            //Debug.Log("1");
            if (cellTypes[i]==12)
            {
                ControlManager.Instance.AllowBJ = true;
                continue;
            }
            //Debug.Log(i);
            InitOneButton(i);

        }
    }
}
