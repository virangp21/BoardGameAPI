using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGame.Domain
{

    public class Board
    {
        public Guid BoardId { get; set; }
        public List<Cell> Cells { get; set; }

        public int Size { get; set; }

        public Board(int size) : this(size, size)
        {

        }
        private Board(int rows, int columns)
        {

            BoardId = Guid.NewGuid();

            Size = rows;

            Cells = new List<Cell>(rows * columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Cells.Add(new Cell()
                    {
                        Row = i,
                        Column = j,
                        RowLabel = (i + 1).ToString(),
                        ColumnLabel = ((char)(j + 65)).ToString(),
                        IsHit = false,
                        IsOccupied = false
                    });
                }
            }
        }


    }
}