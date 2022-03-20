using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Summer
{
    /// <summary>
    /// 用于启动Astar的脚本
    /// </summary>
    public class AstarStart : MonoBehaviour
    {
        public GameObject _node_prefab;
        public GameObject _start_positon;
        public I_AstarMgr _astar_mgr;
        public MapInfo _map_info;
        public Pos _start_pos;
        public Pos _end_pos;
        public int _map_x_length;
        public int _map_y_length;
        public List<Pos> _map_obstacle;
        public bool _build_flag;

        public Button _btn_next_step;
        public Button _btn_find_path;
        public Button _btn_clear;
        public Button _btn_set_start;
        public Button _btn_set_end;
        public Button _btn_set_obs;
        public CameraController _camera_controller;


        private void Start()
        {
            I_AstarMgr astar_mgr = CreatAstar(_map_x_length, _map_y_length, _start_pos, _end_pos, _map_obstacle);

            //设置地图控制
            _btn_next_step.onClick.AddListener(() =>
            {
                NextStep(astar_mgr);
                _refresh_btn_state(astar_mgr);
            });
            _btn_find_path.onClick.AddListener(() =>
            {
                FindPath(astar_mgr);
                _refresh_btn_state(astar_mgr);
            });
            _btn_clear.onClick.AddListener(() =>
            {
                Clear(astar_mgr);
                _refresh_btn_state(astar_mgr);
            });

            //设置射线状态
            _camera_controller._astar_mgr = astar_mgr;
            _btn_set_start.onClick.AddListener(() =>
            {
                _set_cam_state(E_RAYCAST.set_start_node);
                _refresh_btn_state(astar_mgr);
            });
            _btn_set_end.onClick.AddListener(() =>
            {
                _set_cam_state(E_RAYCAST.set_end_node);
                _refresh_btn_state(astar_mgr);
            });
            _btn_set_obs.onClick.AddListener(() =>
            {
                _set_cam_state(E_RAYCAST.set_obs_node);
                _refresh_btn_state(astar_mgr);
            });
        }

        public I_AstarMgr CreatAstar(int map_x_length, int map_y_length, Pos start_pos, Pos end_pos, List<Pos> map_obstacle)
        {
            //1.参数检查
            if (map_x_length < 0)
            {
                Debug.LogWarning("StartAstar() : map_x_length is invalid");
                return null;
            }
            if (map_y_length < 0)
            {
                Debug.LogWarning("StartAstar() : map_x_length is invalid");
                return null;
            }
            if (start_pos == null)
            {
                Debug.LogWarning("StartAstar() : start_pos is null");
                return null;
            }
            if (end_pos == null)
            {
                Debug.LogWarning("StartAstar() : end_pos is null");
                return null;
            }
            if (map_obstacle == null)
            {
                Debug.LogWarning("StartAstar() : map_obstacle is null");
                return null;
            }

            //2.创建可视化控制器
            I_NodeView node_view = new NodeView(_node_prefab, _start_positon.transform.position);

            //3.创建地图
            MapInfo map_info = new MapInfo(map_x_length, map_y_length, _map_obstacle);

            //4.创建AstarMgr
            return new AstarMgr(start_pos, end_pos, map_info, node_view);
        }

        /// <summary>
        /// 执行一步
        /// </summary>
        public void NextStep(I_AstarMgr astar_mgr)
        {
            astar_mgr.NextStep();
        }

        /// <summary>
        /// 一步到位
        /// </summary>
        public void FindPath(I_AstarMgr astar_mgr)
        {
            astar_mgr.FindPath();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Clear(I_AstarMgr astar_mgr)
        {
            astar_mgr.Clear();
        }

        public void _set_cam_state(E_RAYCAST raycast_type)
        {
            _camera_controller._raycast_type = raycast_type;
        }

        /// <summary>
        /// TOFIX:暂时
        /// </summary>
        public void _refresh_btn_state(I_AstarMgr astar_mgr)
        {
            if (astar_mgr.IsPathing())
            {
                _btn_set_end.interactable = false;
                _btn_set_start.interactable = false;
                _btn_set_obs.interactable = false;
                _btn_find_path.interactable = false;
            }
            else
            {
                _btn_set_end.interactable = true;
                _btn_set_start.interactable = true;
                _btn_set_obs.interactable = true;
                _btn_find_path.interactable = true;
            }
        }
    }
}