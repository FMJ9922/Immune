using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartIntroduceUI : MonoBehaviour
{
    public TMP_Text levelName;
    public TMP_Text content;
    public Animation animation;
   

    public void InitText()
    {
        levelName.text = JsonIO.GetLevelName();
        ScoreRequest[] scoreRequests = JsonIO.GetScoreRequest();
        content.text = "" + MyTool.PraseRequest(scoreRequests[0].scoreType, scoreRequests[0].requestNum) + "\n"
                         + MyTool.PraseRequest(scoreRequests[1].scoreType, scoreRequests[1].requestNum) + "\n"
                         + MyTool.PraseRequest(scoreRequests[2].scoreType, scoreRequests[2].requestNum);
    }

    public void OnStartPlayLevelButtonClick()
    {
        
        LevelManager.Instance.StartLevel();
        animation.Play();
    }

}
