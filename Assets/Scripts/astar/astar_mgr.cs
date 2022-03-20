using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Summer
{
    /// <summary>
    /// Astar主要管理类
    /// </summary>
    public class AstarMgr : I_AstarMgr
    {
        public MapInfo _map_info;
        public Node[,] _map;
        public Node _start_node;
        public Node _end_node;
        public OpenList _open_list;
        public CloseList _close_list;
        public I_NodeView _node_view;
        public List<Node> _path;

        public AstarMgr(I_Pos start_point, I_Pos end_point, MapInfo map_info, I_NodeView node_view)
        {
            //1.参数检查
            if (map_info == null)
            {
                Debug.LogWarning("AstarMgr.BuildAstar() : map_info is null");
                return;
            }
            if (start_point == null)
            {
                Debug.LogWarning("AstarMgr.BuildAstar() : start_point is null");
                return;
            }
            if (!_is_valid(start_point, map_info))
            {
                Debug.LogWarning("AstarMgr.BuildAstar() : start_point is invalid");
                return;
            }
            if (end_point == null)
            {
                Debug.LogWarning("AstarMgr.BuildAstar() : end_point is null");
                return;
            }
            if (!_is_valid(end_point, map_info))
            {
                Debug.LogWarning("AstarMgr.BuildAstar() : end_point is invalid");
                return;
            }
            if (node_view == null)
            {
                Debug.LogWarning("AstarMgr.NextStep() : node_view is null");
                return;
            }
            _build_astar(start_point, end_point, map_info, node_view);
        }

        public void _build_astar(I_Pos start_point, I_Pos end_point, MapInfo map_info, I_NodeView node_view)
        {
            //1.设置可视化控制I_NodeView
            _node_view = node_view;

            //2.加载地图
            _load_map(map_info);

            //3.初始化Open_List
            _open_list = new OpenList();

            //4.初始化Close_List
            _close_list = new CloseList();

            //5.设定起始点
            if (!_set_start_node(start_point))
            {
                Debug.LogWarning("_set_start_node faild");
                return;
            };
            if (!_set_end_node(end_point))
            {
                Debug.LogWarning("_set_start_node faild");
                return;
            };

            //6.将初始点加入open_list
            _open_list.Add(_start_node);
        }

        public bool FindPath()
        {
            //参数检查
            if (_start_node == null)
            {
                Debug.LogWarning("AstarMgr.FindPath() : _start_node == null");
                return true;
            }
            if (_end_node == null)
            {
                Debug.LogWarning("AstarMgr.FindPath() : _end_node == null");
                return true;
            }

            //检查是否已经找到路径
            if (_end_node._parent != null)
            {
                Debug.LogWarning("AstarMgr.FindPath() : path already exists");
                return true;
            }

            //循环_next_step
            while (_next_step())
            {
                //若终点有父节点,得到路径,return true,否则继续循环
                if (_end_node._parent != null)
                {
                    List<Node> path = new List<Node>();
                    _get_path(ref path);
                    if (path != null)
                    {
                        _after_get_path(path);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 执行单步
        /// </summary>
        public bool NextStep()
        {
            //参数检查
            if (_start_node == null)
            {
                Debug.LogWarning("AstarMgr.NextStep() : _start_node == null");
                return true;
            }
            if (_end_node == null)
            {
                Debug.LogWarning("AstarMgr.NextStep() : _end_node == null");
                return true;
            }

            //是否已经找到路径
            if (_end_node._parent != null)
            {
                Debug.LogWarning("AstarMgr.NextStep() : path already exists");
                return true;
            }

            //执行_next_step
            _next_step();

            //若终点有父节点,得到路径,return true
            if (_end_node._parent != null)
            {
                List<Node> path = new List<Node>();
                _get_path(ref path);
                if (path != null)
                {
                    _after_get_path(path);
                }
                return true;
            }

            return false;
        }



        /// <summary>
        /// 重建
        /// </summary>
        public void Clear()
        {
            //1.参数检查
            if (_map == null)
            {
                Debug.LogWarning("AstarMgr.Clear() : map is null");
                return;
            }

            //2.全部重新初始化
            foreach (Node node in _map)
            {
                node.Reinit();
            }

            //3.将起点终点设为空
            _start_node = null;
            _end_node = null;

            //3.清空_start_node,_end_node,_open_list,_close_list
            _reinit();
        }

        public void SetStartNode(I_Pos pos)
        {
            //1.参数检查
            if (pos == null)
            {
                Debug.LogWarning("AstarMgr.SetStartNode() : pos is null");
            }

            //2.设定StartNode
            _set_start_node(pos);
        }

        public void SetEndNode(I_Pos pos)
        {
            //1.参数检查
            if (pos == null)
            {
                Debug.LogWarning("AstarMgr.SetEndNode() : pos is null");
            }

            //2.设定StartNode
            _set_end_node(pos);
        }

        public void SetObsNode(I_Pos pos)
        {
            //1.参数检查
            if (pos == null)
            {
                Debug.LogWarning("AstarMgr.SetEndNode() : pos is null");
            }

            //2.设定StartNode
            _set_obs_node(pos);
        }

        public bool IsPathing()
        {
            //判断是否在寻路中
            if (_close_list != null && !_close_list.IsEmpty())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 初始化map
        /// </summary>
        public void _load_map(MapInfo map_info)
        {
            //1.检查地图是否一样
            if (_map_info == map_info)
            {
                return;
            }

            //2.合法性检查
            if (map_info._y_length <= 0 || map_info._y_length > AstarConfig.C_MAX_LENGTH)
            {
                Debug.LogWarning($"AstarMgr.LoadMap() : map_info.length is out of boundary, length:{map_info._y_length}");
                return;
            }
            if (map_info._x_length <= 0 || map_info._x_length > AstarConfig.C_MAX_WIDTH)
            {
                Debug.LogWarning($"AstarMgr.LoadMap() : map_info.width is out of boundary, length:{map_info._x_length}");
                return;
            }

            //3.初始化_map
            _map_info = map_info;

            //4.创建Nodes
            _map = new Node[map_info._x_length, map_info._y_length];
            for (int i = 0; i < map_info._x_length; i++)
            {
                for (int j = 0; j < map_info._y_length; j++)
                {
                    _map[i, j] = new Node(i, j);
                }
            }

            //5.显示地图
            _node_view.CreatNodes(_map);

            //6.设置障碍物
            foreach (I_Pos obstacle in map_info._list_block)
            {
                _map[obstacle.GetX(), obstacle.GetY()].ChangeState(E_NODE_STATE.obstacle);
            }
        }


        /// <summary>
        /// 重新初始化
        /// </summary>
        public void _reinit()
        {
            //新建OpenList和CloseList
            _open_list = new OpenList();
            _close_list = new CloseList();
            _path = null;

            //如果起点存在,将起点加入到OpenList中
            if (_start_node != null && !_open_list.Contains(_start_node))
            {
                _open_list.Add(_start_node);
            }
        }

        /// <summary>
        /// 执行Astar算法单步操作
        /// </summary>
        public bool _next_step()
        {
            // 1. 判断open_list是否为Empty
            if (_open_list.IsEmpty())
            {
                return false;
            }

            // 2. 从open_list中取node
            Node cur_node = _open_list.GetBestNode();

            // 3. 遍历当前最优节点周围节点
            List<Node> list_around_node = new List<Node>();
            _get_around(cur_node, ref list_around_node);
            foreach (Node node in list_around_node)
            {
                // 3.1 判断node节点状态,跳过特殊状态
                if (node._state == E_NODE_STATE.obstacle)
                {
                    continue;
                }
                if (node._state == E_NODE_STATE.close_list)
                {
                    continue;
                }

                // 3.2 判断是否找到目标单
                if (node == _end_node)
                {
                    node._parent = cur_node;
                }

                // 3.3 计算新的_g,_f值更新
                node._h = _cal_h(node, _end_node);
                float new_g = _cal_g(cur_node, node);
                if (new_g < node._g)
                {
                    node._g = new_g;
                    node._parent = cur_node;
                }

                // 3.4 将Node加入OpenList并更新显示状态
                if (!_open_list.Contains(node) && !_close_list.Contains(node) && node._state != E_NODE_STATE.obstacle)
                {
                    _open_list.Add(node);
                    node.ChangeState(E_NODE_STATE.open_list);
                }
            }

            //4.将当前最优节点从open_list中移除,加入close_list
            _close_list.Add(cur_node);
            _open_list.Remove(cur_node);
            cur_node.ChangeState(E_NODE_STATE.close_list);

            return true;
        }

        /// <summary>
        /// 设置起点
        /// </summary>
        public bool _set_start_node(I_Pos pos)
        {
            if (_set_node(pos, ref _start_node))
            {
                _start_node.ChangeState(E_NODE_STATE.start);
                _start_node._g = 0;
                _reinit();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置起始终点
        /// </summary>
        public bool _set_end_node(I_Pos pos)
        {
            if (_set_node(pos, ref _end_node))
            {
                _end_node.ChangeState(E_NODE_STATE.end);
                _reinit();
                return true;
            }
            return false;
        }

        public bool _set_node(I_Pos pos, ref Node node)
        {
            //1.检查合法性
            if (!_is_valid(pos, _map_info))
            {
                Debug.LogWarning("_set_end_node() : pos is invalid");
                return false;
            }

            //2.检查是否是障碍
            Node target_node = _map[pos.GetX(), pos.GetY()];
            if (target_node._state == E_NODE_STATE.obstacle)
            {
                Debug.LogWarning("_set_end_node() : pos is obstacle");
                return false;
            }

            //3.检查原来是否有_end_node
            if (node != null)
            {
                node.ChangeState(E_NODE_STATE.init);
            }

            //4.设置
            node = target_node;
            return true;
        }

        public bool _set_obs_node(I_Pos pos)
        {
            //1.检查合法性
            if (!_is_valid(pos, _map_info))
            {
                Debug.LogWarning("_set_obs_node() : pos is invalid");
                return false;
            }

            //2.检查是否是起始点
            Node target_node = _map[pos.GetX(), pos.GetY()];
            if (target_node._state == E_NODE_STATE.start || target_node._state == E_NODE_STATE.end)
            {
                Debug.LogWarning("_set_obs_node() : pos is invalid");
                return false;
            }
            target_node.ChangeState(E_NODE_STATE.obstacle);

            return true;
        }


        /// <summary>
        /// 得到指定节点周围的节点
        /// </summary>
        public void _get_around(Node node, ref List<Node> list_node)
        {
            //1.得到当前node周围的PosList
            List<I_Pos> pos_list = new List<I_Pos>();
            node._pos.GetAroundPos(ref pos_list);

            //2.遍历_pos_list检验合法性
            foreach (I_Pos pos in pos_list)
            {
                if (_is_valid(pos, _map_info))
                {
                    //3.合法则加入_list_node
                    list_node.Add(_map[pos.GetX(), pos.GetY()]);
                }
            }
        }

        /// <summary>
        /// 判断该Pos是否合法
        /// </summary>
        public bool _is_valid(I_Pos pos, MapInfo map_info)
        {
            //判断pos是否越界
            return !(pos.GetX() < 0 || pos.GetY() < 0 || pos.GetX() >= map_info._x_length || pos.GetY() >= map_info._y_length);
        }

        /// <summary>
        /// 计算G值
        /// </summary>
        public float _cal_g(Node node_source, Node node_around)
        {
            //判断曼哈顿距离,水平竖直方向距离为1,斜对角方向距离为2
            if (_cal_h(node_source, node_around) == AstarConfig.C_INTERVAL_X + AstarConfig.C_INTERVAL_Y)
            {
                //处于对角
                return node_source._g + AstarConfig.C_DIS_CORNER;
            }
            else
            {
                //处于水平、竖直方向
                return node_source._g + AstarConfig.C_DIS_VH;
            }
        }

        /// <summary>
        /// 计算h值：曼哈顿距离
        /// </summary>
        public float _cal_h(Node node_source, Node node_target)
        {
            return Math.Abs(node_source._pos.GetX() - node_target._pos.GetX()) + Math.Abs(node_source._pos.GetY() - node_target._pos.GetY());
        }

        public void _get_path(ref List<Node> path)
        {
            for (Node temp = _end_node; temp != null && temp != _start_node; temp = temp._parent)
            {
                path.Add(temp);
            }
            path.Add(_start_node);
            path.Reverse();
        }

        /// <summary>
        /// 在寻找到路径之后完成对路径的状态改变
        /// </summary>
        public void _after_get_path(List<Node> path)
        {
            foreach (Node node in path)
            {
                node.ChangeState(E_NODE_STATE.path);
            }
            _node_view.AfterGetPath(path);
        }
    }
}