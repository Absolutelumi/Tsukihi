using System.Collections.Generic;

namespace Tsukihi.Chess
{
    internal abstract class Piece
    {
        internal string ImagePath { get; set; }

        internal PlayerType Player { get; set; }

        internal Piece(PlayerType player)
        {
            Player = player;
        }

        internal abstract List<Position> GetMoves(Piece[,] board, Position pos);
    }

    internal enum PlayerType
    {
        Black,
        White
    }
}
