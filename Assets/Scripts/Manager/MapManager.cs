using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject JDImage;
    public GameObject LBImage;
    public GameObject XGImage;

    private void Start()
    {
        JDImage.SetActive(false);
        LBImage.SetActive(false);
        XGImage.SetActive(false);
    }
    public void ShowBg(int level)
    {
        if (level < 5) JDImage.SetActive(true);
        else if(level<9) LBImage.SetActive(true);
        else if(level<13) XGImage.SetActive(true);
    }
}
