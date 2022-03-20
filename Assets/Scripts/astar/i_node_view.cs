using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Summer
{

    public interface I_NodeView
    {
        /// <summary>
        /// 创建所有Node的Cube
        /// </summary>
        void CreatNodes(Node[,] nodes);

        /// <summary>
        /// 改变节点状态
        /// </summary>
        void ChangeShowState(Node node);

        /// <summary>
        /// 完成路径以后的效果
        /// </summary>
        void AfterGetPath(List<Node> list_node);
    }
}