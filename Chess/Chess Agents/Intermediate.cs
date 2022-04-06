using System;
using System.Collections.Generic;
using System.Linq;

namespace Tsukihi.Chess.Chess_Agents
{
    internal class Intermediate : ChessAgent
    {
        private readonly int AbsoluteDepthLimit = 2;
        private int DepthLimit;

        internal Intermediate(PlayerType agent) : base(agent) { }

        private double GetMinMax(Board state, double alpha, double beta, int depth)
        {
            if (depth == DepthLimit) return GetScore(state);

            List<Board> children = state.Next();
            if (children.Count == 0) return GetScore(state);

            foreach (Board child in children)
            {
                double childScore = GetMinMax(child, alpha, beta, depth + 1);

                if (state.Turn == Agent && childScore > alpha) alpha = childScore;
                else if (state.Turn != Agent && childScore < beta) beta = childScore;

                if (beta <= alpha) break;
            }

            return state.Turn == Agent ? alpha : beta;
        }

        protected override Board GetBestMove(List<Board> states)
        {
            Board bestState = states.First();
            DepthLimit = 2;

            while (DepthLimit <= AbsoluteDepthLimit)
            {
                List<Board> bestStates = new List<Board>();
                bestStates.Add(states.First());
                double bestScore = double.NegativeInfinity;

                foreach (Board state in states)
                {
                    double score = GetMinMax(state, Double.NegativeInfinity, Double.PositiveInfinity, 0);

                    if (score == bestScore) bestStates.Add(state);
                    else if (score > bestScore)
                    {
                        bestStates.Clear();
                        bestStates.Add(state);
                        bestScore = score;
                    }
                }

                bestState = bestStates[Random.Next(bestStates.Count)];
                DepthLimit++;
            }

            return bestState;
        }
    }
}
