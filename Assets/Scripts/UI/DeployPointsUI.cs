using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployPointsUI : MonoBehaviour
{
    public Text Deploytext;
    public Text Linbatext;
    public Text KangYuantext;
    private void Start()
    {
        //text = transform.GetComponent<Text>();
    }
    private void Update()
    {
        Deploytext.text = ""+Mathf.FloorToInt(LevelManager.Instance.DeployPoints);
        Linbatext.text = ""+Mathf.FloorToInt(LevelManager.Instance.LinbaPoints);
        KangYuantext.text = ""+Mathf.FloorToInt(LevelManager.Instance.KangYuanPoints);
    }
}
