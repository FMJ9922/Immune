using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJCellControl : AssistCellBase
{
    public override void Produce()
    {
        LevelManager.Instance.AddPoints(atkDamage);
    }
  
    
}
