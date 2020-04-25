using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellButtonUI : MonoBehaviour
{
    public GameObject cellButtonPrefeb;

    private void Start()
    {
        InitButtonSlot();
    }
    private void InitButtonSlot()
    {
        int[] cellTypes = JsonIO.GetAllowCellType();
        int num = cellTypes.Length;
        for (int i = 0; i < num; i++)
        {
            GameObject cellButton = Instantiate(cellButtonPrefeb, transform);
            cellButton.name = ((CellType)cellTypes[i]).ToString();
            cellButton.GetComponent<CellButtonInfo>().cellType = (CellType)cellTypes[i];
            string path = "Cell/" + cellTypes[i] + ((CellType)cellTypes[i]).ToString() + "/" + ((CellType)cellTypes[i]).ToString();
            cellButton.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        }
    }
}
