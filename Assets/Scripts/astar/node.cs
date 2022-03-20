using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Summer
{
    public enum E_NODE_STATE
    {
        none = 0,
        init,
        open_list,
        close_list,
        obstacle,
        path,
        start,
        end,
        max,
    }
    public class Node
    {
        public E_NODE_STATE _state;
        public I_Pos _pos;
        public float _g = float.MaxValue;
        public float _h = float.MaxValue;
        public Node _parent;
        public Action<Node> _change_state_action;
        public Node(int x, int y)
        {
            _pos = new Pos(x, y);
            _state = E_NODE_STATE.init;
        }

        public void ChangeState(E_NODE_STATE state)
        {
            _state = state;
            _change_state_action?.Invoke(this);
        }

        /// <summary>
        /// 重新初始化
        /// </summary>
        public void Reinit()
        {
            _init();
        }

        public void _init()
        {
            ChangeState(E_NODE_STATE.init);
            _parent = null;
            _g = float.MaxValue;
            _h = float.MaxValue;
        }
    }
}