using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGame.Domain
{
    public class Battleship
    {
        public int ShipId { get; set; }
        public int Size { get; set; }

        public int StartRow { get; set; }
        public int StartColumn { get; set; }

        public bool IsHorizontalDirection { get; set; }


    }
}
