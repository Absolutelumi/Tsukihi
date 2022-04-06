using System;
using System.Collections.Generic;

namespace Tsukihi.Chess
{
    internal class Knight : Piece
    {
        internal Knight(PlayerType player) : base(player)
        {
            ImagePath = Tsukihi.ConfigPath + "ChessResources\\" + $"{(Player == PlayerType.White ? "white" : "black")}Knight.png";
        }

        internal override List<Position> GetMoves(Piece[,] board, Position pos)
        {
            List<Position> moves = new List<Position>();

            for (int fileOffset = -2; fileOffset <= 2; fileOffset++)
            for (int rankOffset = -2; rankOffset <= 2; rankOffset++)
                {
                    if (fileOffset == rankOffset || fileOffset == 0 || rankOffset == 0) continue;

                    int file = pos.File + fileOffset;
                    int rank = pos.Rank + rankOffset;
                    if (Math.Min(file, rank) < 0 || Math.Max(file, rank) > 7) continue;
                    if (board[file, rank] != null && board[file, rank].Player == Player) continue;

                    moves.Add(new Position(file, rank));
                }

            return moves;
        }
    }
}
