using System;
using System.Collections.Generic;

namespace Tsukihi.Chess
{
    internal class King : Piece
    {
        internal King(PlayerType player) : base(player)
        {
            ImagePath = Tsukihi.ConfigPath + "ChessResources\\" + $"{(player == PlayerType.White ? "white" : "black")}King.png";
        }

        internal override List<Position> GetMoves(Piece[,] board, Position pos)
        {
            List<Position> moves = new List<Position>();

            for (int fileOffset = -1; fileOffset <= 1; fileOffset++)
            for (int rankOffset = -1; rankOffset <= 1; rankOffset++)
                {
                    int file = pos.File + fileOffset;
                    int rank = pos.Rank + rankOffset;

                    if (Math.Min(file, rank) < 0 || Math.Max(file, rank) > 7) continue;

                    if (board[file, rank] == null ||
                        board[file, rank].Player == board[pos.File, pos.Rank].Player) continue;

                    // Check if puts self in check here

                    moves.Add(new Position(file, rank));
                }

            return moves;
        }
    }
}
