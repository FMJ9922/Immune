using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMotion : MonoBehaviour
{
    public Image dotImage;
    public float scrollSpeed = 0.5F;
    [Range(0f,360f)]
    public float dir;
    private Material mat;
    void Start()
    {
        mat = dotImage.material;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Cos(dir * Mathf.Deg2Rad);
        float y = Mathf.Sin(dir * Mathf.Deg2Rad);
        mat.mainTextureOffset = mat.mainTextureOffset + new Vector2(x, y) * scrollSpeed*0.0001f;
    }
}
