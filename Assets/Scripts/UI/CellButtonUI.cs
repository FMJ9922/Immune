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


    private void Start()
    {
        InitButtonSlot();
        GetComponent<VerticalLayoutGroup>().spacing = 50f;
    }
    private void InitButtonSlot()
    {
        cellTypes = JsonIO.GetAllowCellType();
        num = cellTypes.Length;
        for (int i = 0; i < num; i++)
        {
            GameObject cellButton = Instantiate(cellButtonPrefeb, transform);
            cellButton.name = ((CellType)cellTypes[i]).ToString();
            cellButton.GetComponentInChildren<CellButtonInfo>().cellType = (CellType)cellTypes[i];
            string path = "Cell/" + cellTypes[i] + ((CellType)cellTypes[i]).ToString() + "/" + ((CellType)cellTypes[i]).ToString();
            cellButton.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
            if(cellTypes[i]<7|| cellTypes[i] == 10)
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
    }
}
