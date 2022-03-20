using System;
using System.Collections.Generic;
namespace Summer
{

    public class MapInfo
    {
        public int _x_length;
        public int _y_length;
        public List<Pos> _list_block;

        public MapInfo(int x_length, int y_length, List<Pos> list_block)
        {
            _x_length = x_length;
            _y_length = y_length;
            _list_block = list_block;
        }
    }
}