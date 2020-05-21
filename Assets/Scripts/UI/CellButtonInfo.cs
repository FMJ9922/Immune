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
    public TMP_Text cost;

    private void Start()
    {
  
        cost.text = JsonIO.GetCellData(cellType).initCost.ToString();
         
    }
    public void OnExit()
    {
        ControlManager.Instance.OnPlantButtonSelect(cellType);
    }
    public void OnDown()
    {
        ControlManager.Instance.OnPlantButtonDown();//用于触发锁定屏幕移动
    }
    public void OnClick()
    {
        RoleSelectInfo.Instance.InvalidateInfo(cellType);
        ControlManager.Instance.OnPlantButtonClick(transform);
    }
}
