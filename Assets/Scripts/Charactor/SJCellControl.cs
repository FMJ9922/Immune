using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJCellControl : AssistCellBase
{
    
    public GameObject product;
    public Transform InitPos;

    public override void Produce()
    {
        LevelManager.Instance.AddPoints(PointsType.Deploy,atkDamage);
        GameObject newproduct = Instantiate(product, transform);
        newproduct.transform.position = InitPos.position;
    }
    

}
