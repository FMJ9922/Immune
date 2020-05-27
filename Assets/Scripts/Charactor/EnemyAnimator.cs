using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Sprite[] IdleSprite;
    public Sprite[] DieSprite;
    private EnemyStatus enemyStatus = EnemyStatus.Idle;
    private SpriteRenderer spriteRenderer;
    public EnemyType enemyType;
    int frameP = 0;
    int index = 0;
    void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        index = Random.Range(0, IdleSprite.Length - 1);
    }
    public void PlayAnimation(Sprite[] sprites, int deltaFrame, PlayAnimaType type)
    {
        frameP++;
        if (frameP >= deltaFrame)
        {
            frameP = 0;
            index++;
            if (index >= sprites.Length - 1)
            {
                index = 0;
                if (type  == PlayAnimaType.Fade)
                {
                    //enemyStatus = EnemyStatus.Idle;
                    return;
                }
            }
            spriteRenderer.sprite = sprites[index];
        }
        else return;

    }
    
    public void OnChangeEnemyStatus(EnemyStatus status)
    {
        enemyStatus = status;
        index = 0;
        //Debug.Log(enemyStatus);
    }

    void FixedUpdate()
    {
        switch (enemyStatus)
        {
            case EnemyStatus.Invisable:
                spriteRenderer.sprite = null;
                return;
            case EnemyStatus.Idle:
                PlayAnimation(IdleSprite, 3, PlayAnimaType.Loop);
                break;
            case EnemyStatus.Die:
                PlayAnimation(DieSprite, 2, PlayAnimaType.Loop);
                break;
            case EnemyStatus.Engulfed:
                PlayAnimation(DieSprite, 2, false);
                break;

        }
    }
    public void PlayAnimation(Sprite[] sprites, int deltaFrame, bool fadeIn, PlayAnimaType type = PlayAnimaType.Fade)
    {
        frameP++;
        if (frameP >= deltaFrame)
        {
            frameP = 0;
            index++;
            float alpha = fadeIn ? (index * deltaFrame + frameP) / ((float)sprites.Length * deltaFrame)
            : 1 - (index * deltaFrame + frameP) / ((float)sprites.Length * deltaFrame);
            spriteRenderer.color = new Color(spriteRenderer.color.r,
                                    spriteRenderer.color.g,
                                    spriteRenderer.color.b,
                                    alpha);
            if (index >= sprites.Length - 1)
            {
                index = 0;
                if (type == PlayAnimaType.Fade)
                {
                    enemyStatus = EnemyStatus.Invisable;
                    return;
                }
            }
            spriteRenderer.sprite = sprites[index];
        }
        else return;
    } 
}
