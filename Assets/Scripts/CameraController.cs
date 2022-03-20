using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Summer
{
    public enum E_RAYCAST
    {
        none = 0,
        nil,
        set_end_node,
        set_start_node,
        set_obs_node,
        max
    }

    public class CameraController : MonoBehaviour
    {
        public float _spedd_mouse = 1f;
        public float _speed_hv = 1f;
        public E_RAYCAST _raycast_type = E_RAYCAST.nil;
        public RaycastHit _hit;
        public LayerMask _clickable_layer;
        public I_AstarMgr _astar_mgr;
        void Update()
        {
            //位置移动
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float mouse = Input.GetAxis("Mouse ScrollWheel");
            transform.Translate(new Vector3(h * _speed_hv, -mouse * _spedd_mouse, v * _speed_hv), Space.World);

            //射线检测
            if (_raycast_type == E_RAYCAST.nil)
            {
                return;
            }

            //检测鼠标点击
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out _hit, 50, _clickable_layer.value)) //如果碰撞检测到物体
                {
                    I_Pos pos = _hit.collider.gameObject.GetComponent<TargetNode>()._pos;//打印鼠标点击到的物体名称
                    if (_raycast_type == E_RAYCAST.set_start_node)
                    {
                        _astar_mgr.SetStartNode(pos);
                        _raycast_type = E_RAYCAST.nil;
                    }
                    if (_raycast_type == E_RAYCAST.set_end_node)
                    {
                        _astar_mgr.SetEndNode(pos);
                        _raycast_type = E_RAYCAST.nil;
                    }
                    if (_raycast_type == E_RAYCAST.set_obs_node)
                    {
                        _astar_mgr.SetObsNode(pos);
                        _raycast_type = E_RAYCAST.nil;
                    }
                }
            }
        }
    }
}