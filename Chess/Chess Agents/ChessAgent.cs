using System;
using System.Collections.Generic;

namespace Tsukihi.Chess
{
    internal abstract class ChessAgent
    {
        protected readonly PlayerType Agent;

        protected readonly Random Random;

        internal ChessAgent(PlayerType agent)
        {
            Agent = agent;

            Random = new Random();
        }

        internal Board ChooseMove(Board current) => GetBestMove(current.Next());

        protected abstract Board GetBestMove(List<Board> moves);

        // Material Score
        protected double GetScore(Piece piece)
        {
            if (piece is Pawn) return 1;
            if (piece is Knight) return 3.2;
            if (piece is Bishop) return 3.33;
            if (piece is Rook) return 5.1;
            if (piece is Queen) return 8.8;
            else return 0;
        }

        // Heuristic Score
        protected virtual double GetScore(Board board)
        {
            double score = 0;

            // Material Score for Board
            foreach (Piece piece in board.Pieces) if (piece != null) score += piece.Player == Agent ? GetScore(piece) : -GetScore(piece);

            // Outcome Score
            var outcome = board.InCheck();
            if (outcome.Checkmate) score += board.Turn == Agent ? double.NegativeInfinity : double.PositiveInfinity;

            return score;
        }
    }
}
