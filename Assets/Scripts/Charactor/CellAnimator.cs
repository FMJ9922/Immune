using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CellAnimator : MonoBehaviour
{
    public Sprite[] AtkSprite;//普攻图集
    public Sprite[] IdleSprite;//缓动图集
    public Sprite[] SBSprite;//特殊能力图集
    public Sprite[] DieSprite;//死亡图集
    public Sprite[] ChangeSprite;//变化图集
    public Sprite[] ProduceSprite;//生产图集
    //private CellStatus cellStatus = CellStatus.Invisable;
    private SpriteRenderer spriteRenderer;
    int frameP = 0;
    int index = 0;
    public Direction direction = Direction.Left;
    //private bool doFadeIn = true;
    public delegate void StatusChange(CellStatus cellStatus);
    public event StatusChange OnStatusChange;
    private CellBase cellBase;
    public bool reverse;

    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        cellBase = transform.GetComponentInParent<CellBase>();
        reverse = false;
    }
    void Start()
    {
        
        //transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        //FlipPicture.SaveFlipTextures(IdleSpriteR,CellType.TX);
    }
    public void CleanFrameData()
    {
        frameP = 0;
        index = 0;
        reverse = false;
    }
    public void PlayAnimation(Sprite[] sprites,int deltaFrame, PlayAnimaType type, bool isReverse=false)
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
                if (type == PlayAnimaType.Once&&(sprites == AtkSprite||sprites == SBSprite))
                {
                    if (cellBase.cellType == CellType.TF)
                    {
                        OnStatusChange(CellStatus.Produce);
                    }
                    else
                    {
                        OnStatusChange(CellStatus.Idle);
                    }
                    
                    return;
                }
                else if (type == PlayAnimaType.Once &&sprites == ChangeSprite)
                {
                    if (isReverse) OnStatusChange(CellStatus.Idle);
                    else OnStatusChange(CellStatus.Produce);
                    return;
                }
            }
            spriteRenderer.sprite = isReverse?sprites[sprites.Length-1-index]:sprites[index];
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
                    if (fadeIn)
                    {
                        OnStatusChange(CellStatus.Idle);
                    }
                    else
                    {
                        cellBase.OnDie();
                    }
                    //doFadeIn = false;
                    return;
                }
            }
            spriteRenderer.sprite = sprites[index];
            //spriteRenderer.color = color;
        }
        else return;
    }
    
    void FixedUpdate()
    {
        //Debug.Log(cellBase.cellStatus);
        spriteRenderer.flipX = direction == Direction.Left ? true : false;
        //Debug.Log(cellBase.cellStatus);
        switch (cellBase.cellStatus)
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
            case CellStatus.SpecialAbility:
                PlayAnimation(SBSprite, 2, PlayAnimaType.Once);
                break;
            case CellStatus.Die:
                PlayAnimation(DieSprite, 2, false,PlayAnimaType.Fade);
                break;
            case CellStatus.Change:
                PlayAnimation(ChangeSprite, 2, PlayAnimaType.Once, reverse);
                break;
            case CellStatus.Produce:
                PlayAnimation(ProduceSprite, 2, PlayAnimaType.Loop);
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
