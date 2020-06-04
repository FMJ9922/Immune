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

    private void Start()
    {
        InitButtonSlot();
        GetComponent<VerticalLayoutGroup>().spacing = 7.6f;
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
        }
    }
}
