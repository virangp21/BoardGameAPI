using BoardGame.Domain;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BoardGame.API.Services
{
    public class GameHelperService : IGameHelperService
    {
        public GameHelperService()
        { 
        
        }

        private List<Cell> GetRequiredCells(Battleship ship)
        {
            List<Cell> lstCellsRequired = new List<Cell>();

            if (ship.IsHorizontalDirection)
            {
                for (int i = ship.StartColumn; i < ship.Size; i++)
                {
                    lstCellsRequired.Add(new Cell() { Row = ship.StartRow, Column = i });
                }
            }
            else
            {
                for (int i = ship.StartRow; i < ship.Size; i++)
                {
                    lstCellsRequired.Add(new Cell() { Row = i, Column = ship.StartColumn });
                }
            }

            return lstCellsRequired;
        }
        public bool AreCellsAvailable(Board board, Battleship ship)
        {
            if (board == null || ship == null)
            {
                return false;
            }
            int maxRow = board.Cells.Max(x => x.Row);
            int maxCol = board.Cells.Max(x => x.Column);

            List<Cell> lstCellsRequired = this.GetRequiredCells(ship);

            //Ship outside board
            if (lstCellsRequired.Max(x => x.Row) > maxRow || lstCellsRequired.Max(x => x.Column) > maxCol)
            {
                return false;
            }

            var query = from c in board.Cells
                        join l in lstCellsRequired on new { c.Row, c.Column } equals new { l.Row, l.Column }
                        where c.IsOccupied == true
                        select l;

            //Occupied cells in between required cells
            if (query.Any())
            {
                return false;
            }

            return true;

        }

        public bool AddBattleShip(Board board, Battleship ship)
        {
            if (board == null || ship == null)
            {
                return false;
            }

            List<Cell> lstCellsRequired = this.GetRequiredCells(ship);
            foreach (var cell in lstCellsRequired)
            {
                board.Cells.Where(x => x.Row == cell.Row && x.Column == cell.Column).First().IsOccupied = true;
            }

            return true;
        }

        public bool IsAttacSuccessful(Board board, int row, int column)
        {
            if (board == null)
            {
                return false;
            }

            if (board.Cells.Where(x => x.Row == row && x.Column == column && x.IsHit==false && x.IsOccupied == true ).Any())
            {
                board.Cells.Where(x => x.Row == row && x.Column == column).First().IsHit = true;
                board.Cells.Where(x => x.Row == row && x.Column == column).First().IsOccupied = false;

                return true;
            }
            return false;
        }
    }
}
