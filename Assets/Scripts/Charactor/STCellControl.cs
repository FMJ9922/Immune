using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STCellControl : AssistCellBase
{
    public GameObject product;
    public Transform InitPos;
    public override void Produce()
    {
        LevelManager.Instance.AddPoints(PointsType.KangYuan, atkDamage);
        GameObject newproduct = Instantiate(product, transform);
        newproduct.transform.position = InitPos.position;
    }
    
}
