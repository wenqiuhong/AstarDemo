using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Summer
{
    /// <summary>
    /// 存放Astar配置数据
    /// </summary>
    public class AstarConfig
    {
        public const int C_INTERVAL_X = 1;

        public const int C_INTERVAL_Y = 1;

        /// <summary>
        /// Node在场景中的X轴间距
        /// </summary>
        public const float C_INTERVAL_X_DIS = 1.1f;

        /// <summary>
        /// Node在场景中的Y轴间距
        /// </summary>
        public const float C_INTERVAL_Y_DIS = 1.1f;

        /// <summary>
        /// 地图所接受的最大长度
        /// </summary>
        public const int C_MAX_LENGTH = 200;

        /// <summary>
        /// 地图所接受的最大宽度
        /// </summary>
        public const int C_MAX_WIDTH = 200;

        /// <summary>
        /// 设定上下左右格子的距离
        /// </summary>
        public const float C_DIS_VH = 1.0f;

        /// <summary>
        /// 设定对角格子的距离(由勾股定理计算)
        /// </summary>
        public const float C_DIS_CORNER = 1.4f;

        public const int C_COL_INDEX = 0;
        public const int C_ROW_INDEX = 0;

        //针对DoTween控制
        /// <summary>
        /// 方块升起时间
        /// </summary>
        public const float C_DURATION = 1.0f;

        /// <summary>
        /// 方块升起间隔
        /// </summary>
        public const float C_TIME_INTERVAL = 0.2f;

        /// <summary>
        /// 方块升起高度
        /// </summary>
        public const float C_CUBE_UP_HEIGHT = 0.2f;
    }
}