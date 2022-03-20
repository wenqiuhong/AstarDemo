using System.Collections.Generic;
using UnityEngine;

namespace Summer
{
    /// <summary>
    /// TODO:优化数据结构
    /// </summary>
    public class OpenList
    {
        public List<Node> _list_node;

        public OpenList()
        {
            _list_node = new List<Node>();
        }

        public void Add(Node node)
        {
            //参数检查
            if (node == null)
            {
                Debug.LogWarning("OpenList.Add() : node is null");
            }

            _list_node.Add(node);
        }

        /// <summary>
        /// 得到当前OpenList中最优节点
        /// </summary>
        public Node GetBestNode()
        {
            //遍历OpenList得到最优点
            Node best_node = null;
            foreach (Node node in _list_node)
            {
                if (best_node == null)
                {
                    best_node = node;
                    continue;
                }
                if (node._g + node._h < best_node._g + best_node._h)
                {
                    best_node = node;
                }
            }

            return best_node;
        }

        public Node Remove(Node node)
        {
            //参数检查
            if (node == null)
            {
                Debug.LogWarning("OpenList.Remove() : node is null");
                return null;
            }
            if (!_list_node.Contains(node))
            {
                Debug.LogWarning("OpenList.Remove() : node isn't exist in open_list");
                return null;
            }

            //移除指定Node
            _list_node.Remove(node);

            return node;
        }

        public bool IsEmpty()
        {
            return _list_node.Count == 0;
        }

        public bool Contains(Node node)
        {
            return _list_node.Contains(node);
        }
    }
}

