using System;
using System.Collections.Generic;

namespace Tsukihi.Chess
{
    internal class Board
    {
        internal Piece[,] Pieces { get; private set; }

        internal PlayerType Turn { get; private set; }

        internal string ImagePath { get; private set; }

        internal Board()
        {
            Pieces = ChessExtensions.GetDefaultBoard();

            Turn = PlayerType.White;

            ImagePath = Tsukihi.ConfigPath + "ChessResources\\board.png";
        }

        internal Board(Piece[,] pieces, PlayerType turn)
        {
            Pieces = pieces;

            Turn = turn;

            ImagePath = Tsukihi.ConfigPath + "ChessResources\\board.png";
        }

        internal bool Has<T>(PlayerType player)
        {
            foreach (Piece piece in Pieces) 
                if (piece != null 
                    && piece is T 
                    && piece.Player == player) 
                    return true;

            return false;
        }

        /// <summary>
        /// Attempt to move piece at origin to destination <para/>
        /// Returns true if successful
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        internal bool Move(Position origin, Position destination)
        {
            Piece piece = Pieces[origin.File, origin.Rank];

            if (piece == null) return false;
            var moves = piece.GetMoves(Pieces, origin);
            if (!piece.GetMoves(Pieces, origin).Has(destination)) return false;

            Pieces[origin.File, origin.Rank] = null;
            Pieces[destination.File, destination.Rank] = piece;

            Turn = Turn.Opposite();
            return true;
        }

        /// <summary>
        /// Returns all outcome states from all possible moves
        /// </summary>
        /// <returns></returns>
        internal List<Board> Next() // Arguably should be in ChessExtensions
        {
            List<Board> next = new List<Board>();

            for (int file = 0; file < 8; file++)
            for (int rank = 0; rank < 8; rank++)
                {
                    if (Pieces[file, rank] == null || Pieces[file, rank].Player != Turn) continue;
                    Position origin = new Position(file, rank);

                    foreach (Position destination in Pieces[file, rank].GetMoves(Pieces, origin))
                    {
                        Board board = this.Copy();
                        board.Move(origin, destination);

                        next.Add(board);
                    }
                }

            return next;
        }
    }
}
