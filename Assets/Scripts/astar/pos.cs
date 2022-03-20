using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Summer
{

    public enum E_DIRECTION
    {
        none = 0,
        bottom_left,
        bottom_right,
        top_left,
        top_right,
        left,
        right,
        up,
        down,
        max
    }

    [System.Serializable]
    public class Pos : I_Pos
    {
        public static readonly Dictionary<E_DIRECTION, int[]> C_DICT_DIRECTION = new Dictionary<E_DIRECTION, int[]>()
        {
            {E_DIRECTION.down,new int[]{1,0}},
            {E_DIRECTION.up,new int[]{-1,0}},
            {E_DIRECTION.left,new int[]{0,-1}},
            {E_DIRECTION.right,new int[]{0,1}},
            {E_DIRECTION.bottom_left,new int[]{1,-1}},
            {E_DIRECTION.bottom_right,new int[]{1,1}},
            {E_DIRECTION.top_left,new int[]{-1,-1}},
            {E_DIRECTION.top_right,new int[]{-1,1}}
        };

        public int _x;
        public int _y;

        public Pos(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>
        /// 得到横坐标
        /// </summary>
        public int GetX()
        {
            return _x;
        }

        /// <summary>
        /// 得到纵坐标
        /// </summary>
        public int GetY()
        {
            return _y;
        }

        /// <summary>
        /// 得到当前Pos周围的所有Pos
        /// </summary>
        public void GetAroundPos(ref List<I_Pos> list_pos)
        {
            //1. 参数检查
            if (list_pos == null)
            {
                Debug.LogWarning("Pos.GetAroundPos() : list_pos is null");
                return;
            }

            //2. 遍历_direction_dict
            foreach (var kvp in C_DICT_DIRECTION)
            {
                //3. 根据direction创建不同方向的Pos,加入到list_pos中
                list_pos.Add(new Pos(kvp.Value[0] + _x, kvp.Value[1] + _y));
            }
        }
    }
}