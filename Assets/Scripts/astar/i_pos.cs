using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Summer { }
public interface I_Pos
{
    /// <summary>
    /// 得到当前Index周围的所有索引
    /// </summary>
    void GetAroundPos(ref List<I_Pos> list_pos);

    /// <summary>
    /// 得到横坐标
    /// </summary>
    int GetX();

    /// <summary>
    /// 得到横坐标
    /// </summary>
    int GetY();
}
