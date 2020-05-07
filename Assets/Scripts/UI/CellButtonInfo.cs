using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CellButtonInfo : MonoBehaviour
{
    public CellType cellType;
    public Text name;
    public Text cost;

    private void Start()
    {
        switch (cellType)
        {
            case CellType.SZ:
                name.text = "嗜中性粒细胞";
                cost.text = "花费：" + JsonIO.GetCellData(cellType).initCost.ToString();
                break;
            case CellType.SS:
                name.text = "嗜酸性粒细胞";
                cost.text = "花费：" + JsonIO.GetCellData(cellType).initCost.ToString();
                break;
            case CellType.TX:
                name.text = "效应T细胞";
                cost.text = "花费：" + JsonIO.GetCellData(cellType).initCost.ToString();
                break;
        }
    }
    public void OnExit()
    {
        ControlManager.Instance.OnPlantButtonSelect(cellType);
    }
    public void OnDown()
    {
        ControlManager.Instance.OnPlantButtonDown();//用于触发锁定屏幕移动
    }
}
