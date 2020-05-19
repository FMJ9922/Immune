using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployPointsUI : MonoBehaviour
{
    public Text text;
    private void Start()
    {
        text = transform.GetComponent<Text>();
    }
    private void Update()
    {
        text.text = ""+Mathf.FloorToInt(LevelManager.Instance.DeployPoints);
    }
}
