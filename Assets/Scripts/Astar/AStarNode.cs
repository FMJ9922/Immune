using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : MonoBehaviour
{
    public TileType tileType;
    public int posX;
    public int posY;
    public Vector2 pos;
    public AStarNode parentNode;

    public int costG;
    public int costH;

    public int CostF
    {
        get { return costG + costH; }
    }

    public void InitNode(TileType _tileType,Vector2 _pos,int _x,int _y)
    {
        this.tileType = _tileType;
        this.pos = _pos;
        this.posX = _x;
        this.posY = _y;
    }
}
