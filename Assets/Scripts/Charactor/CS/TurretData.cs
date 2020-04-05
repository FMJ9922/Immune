using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretData : MonoBehaviour
{
    public GameObject turretPrefab;
    public int cost;//价格
    public GameObject turretUpgradedPrefab;//升级
    public int costUpgraded;//升级价格
    public TurretType type;//枚举类型

    public enum TurretType
    {
       
    }
}
