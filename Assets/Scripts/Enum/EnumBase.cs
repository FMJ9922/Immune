using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceType { Null = 0,Selected = 1}
public enum CellStatus { Idle = 0,Attack = 1,CoolDown =2,Upgrade =3,Produce = 4,Invisable =5}
public enum EnemyStatus { Idle = 0,Attack = 1,Bite = 2,Die =3,Invisable = 4}
public enum Direction { Left =0,Right = 1,Up = 2,Down = 3}
public enum TileType { Empty = 0, Block = 1,Occupy=2}

public enum FindPathType 
{
    Defult = 0,//可通过empty,被block和occupy阻挡
    Through = 1,//可通过empty和block ，被occupy阻挡
    Over = 2 //可通过所有TileType
}
public enum CellType
{
    SZ = 0,//嗜中性粒细胞近战
    SS = 1,//嗜酸性粒细胞远程
    SJ = 2,//嗜碱性粒细胞辅助
    JS = 3,//巨噬细胞近战
    ST = 4,//树突细胞辅助
    NK = 5,//NK细胞近战
    TL = 6,//t淋巴细胞
    TX = 7,//效应T近战
    TF = 8,//辅助T辅助
    TY = 9,//抑制性T辅助
    TJ = 10,//记忆T辅助
    BL = 11,//B淋巴
    BJ = 12,//记忆B辅助
    JX = 13//浆细胞远程
}

public enum EnemyType
{
    TH = 0//天花病毒
}

public enum PlayAnimaType
{
    Loop = 0,
    Once = 1,
    Fade = 2,
    Null = 3

}