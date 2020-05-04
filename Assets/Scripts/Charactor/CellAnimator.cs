using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CellAnimator : MonoBehaviour
{
    public Sprite[] AtkSprite;
    public Sprite[] IdleSprite;
    private CellStatus cellStatus = CellStatus.Invisable;
    private SpriteRenderer spriteRenderer;
    private CellType cellType;
    int frameP = 0;
    int index = 0;
    public Direction direction = Direction.Left;
    private bool doFadeIn = true;
    
    void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        cellType = transform.GetComponentInParent<CellBase>().cellType;
        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.position.y);

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
        Debug.Log(alpha);
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
                    doFadeIn = false;
                    return;
                }
            }
            spriteRenderer.sprite = sprites[index];
            //spriteRenderer.color = color;
        }
        else return;
    }
    public void OnChangeCellStatus(CellStatus status,bool resetIndex)
    {
        cellStatus = status;
        if (resetIndex) { index = 0; }
    }
    void FixedUpdate()
    {
        spriteRenderer.flipX = direction == Direction.Left ? true : false;
       
        switch (cellStatus)
        {
            case CellStatus.Invisable:
                PlayAnimation(IdleSprite, 2, true, PlayAnimaType.Fade);
                return;
            case CellStatus.Idle:
                PlayAnimation(IdleSprite,2,PlayAnimaType.Loop);
                break;
            case CellStatus.Attack:
                PlayAnimation(AtkSprite, 2, PlayAnimaType.Once);
                break;

        }
    }
    [ContextMenu("SetUp")]
    public void SetUpPicture()
    {
        string path = "Cell/0SZ/Idle/嗜中缓动00";
        for(int i = 0; i < 36; i++)
        {
            if (i < 9)
            {
                IdleSprite[i] = Resources.Load(path + "0" + (i+1).ToString()) as Sprite;
                Debug.Log(path + "0" + (i + 1).ToString());
            }
            else
            {
                IdleSprite[i] = Resources.Load(path + (i + 1).ToString()) as Sprite;
            }
        }

    }
    void load()
    {
        List<string> filePaths = new List<string>();
        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');
        for (int i = 0; i < ImageType.Length; i++)
        {
            //获取d盘中a文件夹下所有的图片路径
            string[] dirs = Directory.GetFiles(@"d:\\a", ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                filePaths.Add(dirs[j]);
            }
        }

        for (int i = 0; i < filePaths.Count; i++)
        {
            Texture2D tx = new Texture2D(100, 100);
            tx.LoadImage(getImageByte(filePaths[i]));
            //allTex2d.Add(tx);
        }
    }

    /// <summary>
    /// 根据图片路径返回图片的字节流byte[]
    /// </summary>
    /// <param name="imagePath">图片路径</param>
    /// <returns>返回的字节流</returns>
    private static byte[] getImageByte(string imagePath)
    {
        FileStream files = new FileStream(imagePath, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
        return imgByte;
    }
}
