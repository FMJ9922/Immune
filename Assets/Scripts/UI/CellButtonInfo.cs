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
        ControlManager.Instance.OnButtonSelect(cellType);
    }
    public void OnDown()
    {
        ControlManager.Instance.isSelect = true;
    }
}
