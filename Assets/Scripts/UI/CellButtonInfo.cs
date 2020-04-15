using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class CellButtonInfo : MonoBehaviour
{
    public CellType cellType;
  
    public void OnExit()
    {
        ControlManager.Instance.OnPlantButtonSelect(cellType);
    }
    public void OnDown()
    {
        ControlManager.Instance.OnPlantButtonDown();//用于触发锁定屏幕移动
    }
}
