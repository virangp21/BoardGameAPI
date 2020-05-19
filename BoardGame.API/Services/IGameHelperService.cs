using BoardGame.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGame.API.Services
{
    public interface IGameHelperService
    {
        public bool AreCellsAvailable(Board b, Battleship ship);

        public bool AddBattleShip(Board b, Battleship ship);

        public bool IsAttacSuccessful(Board b, int row, int column);

    }
}
