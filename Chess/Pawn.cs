using System;
using System.Collections.Generic;

namespace Tsukihi.Chess
{
    internal class Pawn : Piece
    {
        internal Pawn(PlayerType player) : base (player)
        {
            ImagePath = Tsukihi.ConfigPath + "ChessResources\\" + $"{(player == PlayerType.White ? "white" : "black")}Pawn.png";
        }

        internal override List<Position> GetMoves(Piece[,] board, Position pos)
        {
            List<Position> moves = new List<Position>();

            // Moving forward
            int rankOffset = Player == PlayerType.White ? 1 : -1;
            if (board[pos.File, pos.Rank + rankOffset] == null) 
                moves.Add(new Position(pos.File, pos.Rank + rankOffset));

            // Moving forward 2 on starting move
            rankOffset = Player == PlayerType.White ? 2 : -2;
            if (((Player == PlayerType.White && pos.Rank == 1) || (Player == PlayerType.Black && pos.Rank == 6)) &&
                board[pos.File, pos.Rank + rankOffset] == null)
                moves.Add(new Position(pos.File, pos.Rank + rankOffset));

            // Capturing piece
            rankOffset = Player == PlayerType.White ? 1 : -1;

            if (Math.Min(pos.File + 1, pos.Rank + rankOffset) >= 0 &&
                Math.Max(pos.File + 1, pos.Rank + rankOffset) <= 7 && 
                board[pos.File + 1, pos.Rank + rankOffset] != null &&
                board[pos.File + 1, pos.Rank + rankOffset].Player != Player)
                moves.Add(new Position(pos.File + 1, pos.Rank + rankOffset));

            if (Math.Min(pos.File - 1, pos.Rank + rankOffset) >= 0 &&
                Math.Max(pos.File - 1, pos.Rank + rankOffset) <= 7 &&
                board[pos.File - 1, pos.Rank + rankOffset] != null &&
                board[pos.File - 1, pos.Rank + rankOffset].Player != Player)
                moves.Add(new Position(pos.File - 1, pos.Rank + rankOffset));

            // EnPassant

            return moves;
        }
    }
}
