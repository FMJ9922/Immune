using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulding : MonoBehaviour
{
    public GameObject turretPrefab;
    public int cost = 10;//价格
    public GameObject turretUpgradedPrefab;//升级
    public int costUpgraded = 20;//升级价格
  
    public Text dpText;

    public float dp = 25;

    public GameObject upgradeCanvas;

    public Button buttonUpgrade;

    public Button cellUpgrade;

    void ChangeDP(int change = 0)
    {
        dp += change;
        dpText.text = "dp";
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    public void OnUpgradeButtonDown()
    {
        if (dp >= costUpgraded)
        {
            ChangeDP(-costUpgraded);
            turretUpgradedPrefab.SetActive(true);
            turretPrefab.SetActive(false);

        }

        else {
            Debug.Log("没钱");
        }
    }

    public void OncellUpgradeDown()
    {
        upgradeCanvas.SetActive(true);
    }
}
