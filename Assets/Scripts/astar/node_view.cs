using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading;

namespace Summer
{

    public class NodeView : I_NodeView
    {
        public static readonly Dictionary<E_NODE_STATE, Color> C_DICT_COLOR = new Dictionary<E_NODE_STATE, Color>()
        {
            { E_NODE_STATE.init,Color.white },
            { E_NODE_STATE.start, Color.yellow },
            { E_NODE_STATE.end, Color.red },
            { E_NODE_STATE.open_list, Color.cyan },
            { E_NODE_STATE.path, Color.green },
            { E_NODE_STATE.close_list, Color.black},
        };

        public Node[,] _nodes_map;
        public GameObject[,] _nodes_view_map;
        public GameObject _prefab;
        public Vector3 _start_position;

        public NodeView(GameObject prefab, Vector3 start_position)
        {
            _prefab = prefab;
            _start_position = start_position;
        }

        /// <summary>
        /// 创建所有Node的Cube
        /// </summary>
        public void CreatNodes(Node[,] nodes)
        {
            //1.参数检查
            if (nodes == null)
            {
                Debug.LogWarning("NodeView.CreatNodes() : nodes is null");
                return;
            }
            if (_nodes_view_map != null)
            {
                _clear();
            }
            _nodes_map = nodes;

            //2.创建nodes
            int x_length = nodes.GetLength(AstarConfig.C_COL_INDEX);
            int y_length = nodes.GetLength(AstarConfig.C_ROW_INDEX);
            _nodes_view_map = new GameObject[x_length, y_length];
            for (int i = 0; i < x_length; i++)
            {
                for (int j = 0; j < y_length; j++)
                {
                    _nodes_view_map[i, j] = GameObject.Instantiate(_prefab);
                    TargetNode target_node = _nodes_view_map[i, j].AddComponent<TargetNode>();

                    //设置显示状态
                    float x = _start_position.x + i * AstarConfig.C_INTERVAL_X_DIS;
                    float z = _start_position.z + j * AstarConfig.C_INTERVAL_Y_DIS;
                    _nodes_view_map[i, j].transform.position = new Vector3(x, _nodes_view_map[i, j].transform.position.y, z);
                    nodes[i, j]._change_state_action += ChangeShowState;
                    target_node._pos = nodes[i, j]._pos;
                }
            }
        }

        /// <summary>
        /// 改变节点状态
        /// </summary>
        public void ChangeShowState(Node node)
        {
            //1.参数检查
            if (_nodes_view_map == null)
            {
                Debug.LogWarning("NodeView.ChangeColor() : nodes_view_map is null");
                return;
            }
            if (node == null)
            {
                Debug.LogWarning("NodeView.ChangeColor() : node is null");
                return;
            }
            if (node._pos.GetX() > _nodes_view_map.GetLength(AstarConfig.C_COL_INDEX)
             || node._pos.GetY() > _nodes_view_map.GetLength(AstarConfig.C_ROW_INDEX))
            {
                Debug.LogWarning("NodeView.ChangeColor() : node is out of boundary");
                return;
            }

            //2.如果Node为Obstacle,隐藏
            GameObject target_go = _nodes_view_map[node._pos.GetX(), node._pos.GetY()];
            if (node._state == E_NODE_STATE.obstacle)
            {
                target_go.SetActive(false);
                return;
            }
            else
            {
                target_go.SetActive(true);
            }

            //3.改变颜色
            if (target_go == null)
            {
                Debug.LogWarning("NodeView.ChangeColor() : gameobject isn't exist");
                return;
            }
            _nodes_view_map[node._pos.GetX(), node._pos.GetY()].GetComponent<MeshRenderer>().material.color = C_DICT_COLOR[node._state];

        }

        public void AfterGetPath(List<Node> list_node)
        {
            for (int i = 0; i < list_node.Count; i++)
            {
                GameObject go = _get_go_by_node(list_node[i]);
                Vector3 vector = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);

                //使用Dotween显示路径
                float time_count = 0;
                DOTween.To(() => time_count, a => time_count = a, AstarConfig.C_DURATION, i * AstarConfig.C_TIME_INTERVAL).
                    OnComplete(() => go.transform.DOPunchPosition(new Vector3(0, AstarConfig.C_CUBE_UP_HEIGHT, 0), 1, 1, 0));
            }
        }

        public GameObject _get_go_by_node(Node node)
        {
            return _nodes_view_map[node._pos.GetX(), node._pos.GetY()];
        }

        public void _clear()
        {
            foreach (GameObject go in _nodes_view_map)
            {
                GameObject.Destroy(go);
            }
        }
    }
}