using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    public Sprite goldStar;
    public Sprite greyStar;
    private Image[] starImages;
    void Start()
    {
        starImages = GetComponentsInChildren<Image>();
        int score = (PlayerPrefs.GetInt("LevelScore" + transform.parent.name, 0));
        for(int i = 0;i < starImages.Length; i++)
        {
            starImages[i].overrideSprite = i<score ? goldStar : greyStar;
        }
        
    }

    
}
