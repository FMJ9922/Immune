using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FlipPicture : MonoBehaviour
{
    // Start is called before the first frame update
    public static Texture2D horizontalFlipPic(Texture2D texture2d)
    {
        int width = texture2d.width;//得到图片的宽度.   
        int height = texture2d.height;//得到图片的高度 

        Texture2D NewTexture2d = new Texture2D(width, height);//创建一张同等大小的空白图片 

        int i = 0;

        while (i < width)
        {
            NewTexture2d.SetPixels(i, 0, 1, height, texture2d.GetPixels(width - i - 1, 0, 1, height));
            i++;
        }
        NewTexture2d.Apply();

        return NewTexture2d;
    }

    public static void SaveFlipTextures(Texture2D[] textures,CellType cellType)
    {
        for(int i = 0; i < textures.Length; i++)
        {
            Texture2D texture2D = horizontalFlipPic(textures[i]);
            string path = Application.streamingAssetsPath + "/"+textures[i].name+"Flip.png";
            SaveTextureAsPNG(texture2D, path);
        }
    }

    public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        //Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
    }

}
