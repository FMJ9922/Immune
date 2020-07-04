using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceType { Null = 0,Selected = 1,Remove = 2}
public enum CellStatus { Idle = 0,Attack = 1,Change =2,Upgrade =3,Produce = 4, Invisable = 5,SpecialAbility = 6,Die =7}
public enum EnemyStatus { Idle = 0,Attack = 1,Bite = 2,Die =3,Invisable = 4,Engulfed = 5}
public enum Direction { Left =0,Right = 1,Up = 2,Down = 3}
public enum TileType { Empty = 0, Block = 1,Occupy=2,Normal = 3}

public enum ArticleType
{
    //类型
    SRCell,
    LRCell,
    HelpCell,
    Enemy,
    Cell
}
public enum FindPathType 
{
    Defult = 0,//可通过empty,被block和occupy阻挡
    Through = 1,//可通过empty和block ，被occupy阻挡
    Over = 2 //可通过所有TileType
}
public enum CellType
{
    SZ = 0,//嗜中性粒细胞 近战
    SS = 1,//嗜酸性粒细胞 远程
    SJ = 2,//嗜碱性粒细胞 辅助
    JS = 3,//巨噬细胞     近战
    NK = 4,//NK细胞       远程
    ST = 5,//树突细胞     辅助
    TL = 6,//t淋巴细胞    辅助
    TX = 7,//效应T        近战
    TF = 8,//辅助T        辅助
    TY = 9,//抑制性T      辅助
    BL = 10,//B淋巴       辅助
    JX = 11,//浆细胞      远程1
    BJ = 12,//记忆B       辅助
}

public enum EnemyType
{
    BD = 0,//病毒
    JSC = 1,//寄生虫
    XJ = 2,//细菌
    SZ = 3,//宿主细胞
    LXT = 4,//螺旋体
    ZYT = 5,//支原体
    GMY = 6,//过敏原
}
public enum ActorType
{
    SZ = 0,//嗜中性粒细胞 近战
    SS = 1,//嗜酸性粒细胞 远程
    SJ = 2,//嗜碱性粒细胞 辅助
    JS = 3,//巨噬细胞     近战
    NK = 4,//NK细胞       远程
    ST = 5,//树突细胞     辅助
    TL = 6,//t淋巴细胞    辅助
    TX = 7,//效应T        近战
    TF = 8,//辅助T        辅助
    TY = 9,//抑制性T      辅助
    BL = 10,//B淋巴       辅助
    JX = 11,//浆细胞      远程
    BJ = 12,//记忆B       辅助
    QT=13,
    TH = 14,//天花病毒
    EB = 15,//eb
    BD = 16,//病毒宿主
    HJ = 17,//黄金葡萄球菌
    JXB = 18,//巨细胞病毒
    MD = 19,//梅毒螺旋体
    //JSC=20,//寄生虫
    ZC=20,
  //  ZC = 21,//正常细胞
  //  QT = 22,//其他正常细胞





}
public enum PlayAnimaType
{
    Loop = 0,
    Once = 1,
    Fade = 2,
    Null = 3

}
public enum FireMode
{
    First = 0,
    Weakest = 2,
    Nearest = 1,
    Strongest = 3,
}

public enum AttackType
{
    Swallow = 0,//吞噬
    Other = 1,//其它
}

public enum ScoreType
{
    EnemyEscapeNum = 0,//敌人的逃离数量 小于
    CellDeployNum = 1,//细胞的放置数量 小于
    EnemyRouteLength = 3,//敌人行进距离 大于
    NormalCellSurviveNum = 2,//健康细胞存活数量 小于

}
public enum SceneMap
{
    JD=0,//结缔组织
    LB=1,//淋巴
    XG=2,//血管
}
public enum PointsType
{
    Deploy = 0,
    LinBa = 1,
    KangYuan = 2,
}

public enum SoundResource
{
    sfx_bomb = 0,//炸弹
    sfx_btnN = 1,//取消
    sfx_btnY = 2,//确认
    sfx_cell_die = 3,
    sfx_countdown = 4,//倒计时
    sfx_enemy_dead = 5,//敌人死亡
    sfx_enemy_escape = 6,//敌人逃离
    sfx_enemy_hit1 = 7,//敌人掉血
    sfx_fail = 8,//失败
    sfx_gain_bushu = 9,//部署点数
    sfx_gain_kangti = 10,//部署点数
    sfx_gain_linba = 11,//部署点数
    sfx_page_turn = 12,
    sfx_root_out = 13,
    sfx_shovel_select = 14,//选中铲子
    sfx_start = 15,//开始
    sfx_success = 16,//成功
    sfx_upgrade = 17,//细胞升级

}