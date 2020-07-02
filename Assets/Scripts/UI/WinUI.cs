using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public TMP_Text levelName;
    public TMP_Text content;
    public Sprite goldStar;
    public Sprite greyStar;
    public Image[] starImages;
    public Image[] starImages1;

    public void InitWinUI(ScoreRequest[] scoreRequests)
    {
        levelName.text = JsonIO.GetLevelName();
        int achieveNum = 0;
        for(int i = 0; i < scoreRequests.Length; i++)
        {
            //异或
            bool achieve = !(((int)scoreRequests[i].scoreType < 2 && !(scoreRequests[i].actualNum <= scoreRequests[i].requestNum))
                            || (!((int)scoreRequests[i].scoreType < 2) && (scoreRequests[i].actualNum <= scoreRequests[i].requestNum)));
            /*Debug.Log(((int)scoreRequests[i].scoreType < 2 && !(scoreRequests[i].actualNum <= scoreRequests[i].requestNum))+" "+
                (!((int)scoreRequests[i].scoreType < 2) && (scoreRequests[i].actualNum <= scoreRequests[i].requestNum)) + " " +
                achieve);*/
            starImages[i].overrideSprite = achieve ? goldStar:greyStar;
            achieveNum = achieve ? achieveNum + 1 : achieveNum;
        }
        if (achieveNum == 0)
        {
            LevelManager.Instance.ShowWinOrFailCanvas(false);
            return;
        }
        PlayerPrefs.SetInt("LevelScore" + GameManager.Instance.iLevel,achieveNum);
        //Debug.Log(achieveNum);
        for (int i = 0; i < scoreRequests.Length; i++)
        {
            starImages1[i].overrideSprite = i<achieveNum ? goldStar : greyStar;
        }

          
    }
    private void InitText()
    {
        levelName.text = JsonIO.GetLevelName();
        ScoreRequest[] scoreRequests = JsonIO.GetScoreRequest();
        content.text = "" + MyTool.PraseRequest(scoreRequests[0].scoreType, scoreRequests[0].requestNum) + "\n"
                         + MyTool.PraseRequest(scoreRequests[1].scoreType, scoreRequests[1].requestNum) + "\n"
                         + MyTool.PraseRequest(scoreRequests[2].scoreType, scoreRequests[2].requestNum);
    }
}
