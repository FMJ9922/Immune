using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoleSelectInfo : MonoBehaviour
{
    public static RoleSelectInfo Instance { get; private set; } = null;
    void Awake()
    {
        Instance = this;
        cellName = transform.Find("RoleName").GetComponent<TMP_Text>();
        cellAtk = transform.Find("AtkInfo").GetComponent<TMP_Text>();
        cellRate = transform.Find("RateInfo").GetComponent<TMP_Text>();
        cellRange = transform.Find("RangeInfo").GetComponent<TMP_Text>();
        cellCost = transform.Find("CostInfo").GetComponent<TMP_Text>();
        cellType = transform.Find("TypeInfo").GetComponent<TMP_Text>();
        cellAbility = transform.Find("AbilityInfo").GetComponent<TMP_Text>();
        cellIntro = transform.Find("IntroduceInfo").GetComponent<TMP_Text>();

    }
    private CellType lastcellType;
    private TMP_Text cellName;
    private TMP_Text cellAtk;
    private TMP_Text cellRate;
    private TMP_Text cellRange;
    private TMP_Text cellCost;
    private TMP_Text cellType;
    private TMP_Text cellAbility;
    private TMP_Text cellIntro;
    public void ChangeInstance(Transform trans, CellType cellType)
    {
        if (cellType != lastcellType)
        {
            GameObject newInstance = Instantiate(this.gameObject, transform.parent);
            newInstance.transform.position = trans.position;
            newInstance.name = transform.name;
            this.transform.GetComponent<Animation>().Play("HideCellInfo");
            Invoke("DestroyThis", 1f);
            Instance = newInstance.GetComponent<RoleSelectInfo>();
            Instance.transform.GetComponent<Animation>().Play("ShowCellInfo");
            Instance.InvalidateInfo(cellType);
        }
        
    }
    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
    void Start()
    {
        
    }

    
    

    public void InvalidateInfo(CellType cellType)
    {
            cellName.text = JsonIO.GetCellData(cellType).name;
            cellAtk.text = "伤害:" + JsonIO.GetCellData(cellType).atkDamage.ToString();
            cellRate.text = "冷却:" + JsonIO.GetCellData(cellType).atkDuration.ToString() + "秒";
            cellRange.text = "范围:" + JsonIO.GetCellData(cellType).atkRange.ToString();
            cellCost.text = "花费:" + JsonIO.GetCellData(cellType).initCost.ToString();
            this.cellType.text = "定位:" + JsonIO.GetCellData(cellType).type;
            cellAbility.text = "技能:" + JsonIO.GetCellData(cellType).ability;
            cellIntro.text = "简介:" + JsonIO.GetCellData(cellType).introduce;
        lastcellType = cellType;
    }
}
