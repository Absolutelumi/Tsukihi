using System;
using System.Collections.Generic;

namespace Tsukihi.Chess
{
    internal class Bishop : Piece
    {
        internal Bishop(PlayerType player) : base(player)
        {
            ImagePath = Tsukihi.ConfigPath + "ChessResources\\" + $"{(player == PlayerType.White ? "white" : "black")}Bishop.png";
        }

        internal override List<Position> GetMoves(Piece[,] board, Position pos)
        {
            List<Position> moves = new List<Position>();

            for (int fileOffset = -1; fileOffset <= 1; fileOffset++)
            for (int rankOffset = -1; rankOffset <= 1; rankOffset++)
                {
                    if (fileOffset == 0 || rankOffset == 0) continue;

                    int file = pos.File + fileOffset;
                    int rank = pos.Rank + rankOffset;
                    while (Math.Min(file, rank) >= 0 && Math.Max(file, rank) <= 7)
                    {
                        if (board[file, rank] != null)
                        {
                            if (board[file, rank].Player != Player) moves.Add(new Position(file, rank));
                            break;
                        }
                        moves.Add(new Position(file, rank));

                        file += fileOffset;
                        rank += rankOffset;
                    }
                }

            return moves;
        }
    }
}
