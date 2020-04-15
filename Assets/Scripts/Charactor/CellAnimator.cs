using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAnimator : MonoBehaviour
{
    public Sprite[] AtkSpriteL;
    public Sprite[] AtkSpriteR;
    public Sprite[] IdleSpriteL;
    public Sprite[] IdleSpriteR;
    private CellStatus cellStatus = CellStatus.Invisable;
    private SpriteRenderer spriteRenderer;
    private CellType cellType;
    int frameP = 0;
    int index = 0;
    public Direction direction = Direction.Left;
    private bool doFade = true;
    
    void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        cellType = transform.GetComponentInParent<CellBase>().cellType;

        //FlipPicture.SaveFlipTextures(IdleSpriteR,CellType.TX);
    }
    /*private Texture2D[] LoadTextureFromFile()
    {

        
    }*/
    public void PlayAnimation(Sprite[] sprites,int deltaFrame, PlayAnimaType type)
    {
        if (sprites.Length <= 0) return;
        frameP++;
        if (frameP >= deltaFrame)
        {
            frameP = 0;
            index++;
            if (index >= sprites.Length - 1)
            {
                index = 0;
                if (type == PlayAnimaType.Once)
                {
                    cellStatus = CellStatus.Idle;
                    return;
                }
            }
            spriteRenderer.sprite = sprites[index];
        }
        else return;

    }
    public void PlayAnimation(Sprite[] sprites, int deltaFrame, bool fadeIn, PlayAnimaType type = PlayAnimaType.Fade)
    {
        
        frameP++;
        float alpha = fadeIn ? (index*deltaFrame+frameP) / ((float)sprites.Length*deltaFrame)
            : 1 - (index * deltaFrame + frameP) / ((float)sprites.Length * deltaFrame);
        //Debug.Log(alpha);
        spriteRenderer.color = new Color(spriteRenderer.color.r,
                                spriteRenderer.color.g,
                                spriteRenderer.color.b,
                                alpha);
        if (frameP >= deltaFrame)
        {
            frameP = 0;
            index++;
            
            
            if (index >= sprites.Length)
            {
                index = 0;
                if (type == PlayAnimaType.Fade)
                {
                    cellStatus = CellStatus.Idle;
                    doFade = false;
                    return;
                }
            }
            spriteRenderer.sprite = sprites[index];
            //spriteRenderer.color = color;
        }
        else return;
    }
    public void OnChangeCellStatus(CellStatus status)
    {
        cellStatus = status;
        index = 0;
    }
    void FixedUpdate()
    {
        if (doFade)
        {
            PlayAnimation(IdleSpriteL, 8, true);
            
        }
        switch (cellStatus)
        {
            case CellStatus.Invisable:
                
                return;
            case CellStatus.Idle:
                if(direction == Direction.Left)
                    PlayAnimation(IdleSpriteL,15,PlayAnimaType.Loop);
                else if(direction == Direction.Right)
                    PlayAnimation(IdleSpriteR, 15, PlayAnimaType.Loop);

                break;
            case CellStatus.Attack:
                if (direction == Direction.Left)
                    PlayAnimation(AtkSpriteL, 5, PlayAnimaType.Once);
                else if (direction == Direction.Right)
                    PlayAnimation(AtkSpriteR, 5, PlayAnimaType.Once);
                break;

        }
    }
}
