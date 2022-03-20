using System.Collections.Generic;
using UnityEngine;

namespace Summer
{
    /// <summary>
    /// TODO:优化数据结构
    /// </summary>
    public class CloseList
    {
        List<Node> _list_node;

        public CloseList()
        {
            _list_node = new List<Node>();
        }

        public void Add(Node node)
        {
            //1.参数检查
            if (node == null)
            {
                Debug.LogWarning("CloseList.Add() : node is null");
                return;
            }

            //2.将Node加入list中
            _list_node.Add(node);
        }

        public bool Contains(Node node)
        {
            return _list_node.Contains(node);
        }

        public bool IsEmpty()
        {
            return _list_node.Count == 0;
        }
    }
}