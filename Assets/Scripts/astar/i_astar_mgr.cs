using System.Collections;
using System.Collections.Generic;
using System;
namespace Summer
{
    public interface I_AstarMgr
    {
        /// <summary>
        /// 执行一步
        /// </summary>
        bool NextStep();

        /// <summary>
        /// 全部执行
        /// </summary>
        bool FindPath();

        /// <summary>
        /// 清空地图信息
        /// </summary>
        void Clear();

        void SetStartNode(I_Pos pos);

        void SetEndNode(I_Pos pos);

        void SetObsNode(I_Pos pos);

        /// <summary>
        /// 是否在寻路中
        /// </summary>
        bool IsPathing();
    }
}