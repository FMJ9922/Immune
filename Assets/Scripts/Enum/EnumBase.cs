using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Null = 0}
public enum PlaceType { Null = 0,Selceted = 1}
public enum CellStatus { Idle = 0,Attack = 1,CoolDown =2,Upgrade =3,Produce = 4}

public enum TileType { Empty = 0, Block = 1,Occupy=2}

public enum FindPathType 
{
    Defult = 0,//可通过empty,被block和occupy阻挡
    Through = 1,//可通过empty和block ，被occupy阻挡
    Over = 2 //可通过所有TileType
}
public enum CellType
{
    SZ = 0,//嗜中性粒细胞
    SS = 1,//嗜酸性粒细胞
    SJ = 2,//嗜碱性粒细胞
    JS = 3,//巨噬细胞
    ST = 4,//树突细胞
    NK = 5,//NK细胞
    TL = 6,//t淋巴细胞
    TX = 7,//效应T
    TF = 8,//辅助T
    TY = 9,//抑制性T
    TJ = 10,//记忆T
    BL = 11,//B淋巴
    BJ = 12,//记忆B
    JX = 13//浆细胞
}