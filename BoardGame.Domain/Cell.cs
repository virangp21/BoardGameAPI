using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public string RowLabel { get; set; }

        public string ColumnLabel { get; set; }

        public bool IsHit { get; set; }

        public bool IsOccupied { get; set; }
    }
}
