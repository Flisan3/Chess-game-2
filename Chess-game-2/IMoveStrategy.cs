using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Chess_game_2
{
    // Interface for move strategies of different chess pieces
    internal interface IMoveStrategy
    {
        List<(int row, int col)> GetValidMoves(int row, int col, string[,] positions);
    }
}
