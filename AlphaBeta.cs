using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaBotImplementation
{
    public class AlphaBeta
    {
        bool maxPlayer = true;
        public AlphaBeta()
        {
        }
        public MoveValue Iterate(GameState state, int depth, double alpha, double beta, bool player)
        {
            MoveValue moveValue;
            MoveValue bestMove = null;

            if (depth == 0 || state.IsEnd())
            {
                return new MoveValue(state.GetTotalHeuristic());
            }

            if (player == maxPlayer)
            {
                foreach (GameState child in state.GenerateMoves(player))
                {
                    moveValue = Iterate(child, depth - 1, alpha, beta, !player);
                    if (bestMove == null || (moveValue.getValue() > bestMove.getValue()))
                    {
                        bestMove = new MoveValue(moveValue.getValue(), new Move(state, child));
                        alpha = bestMove.getValue();
                    }

                    if (beta <= alpha)
                    {
                        return new MoveValue(alpha);
                    }

                }
                return bestMove;
            }
            else
            {
                foreach (GameState child in state.GenerateMoves(player))
                {
                    moveValue = Iterate(child, depth - 1, alpha, beta, !player);
                    if (bestMove == null || (moveValue.getValue() < bestMove.getValue()))
                    {
                        bestMove = new MoveValue(moveValue.getValue(), new Move(state, child));
                        beta = bestMove.getValue();
                    }

                    if (beta <= alpha)
                    {
                        return new MoveValue(beta);
                    }
                }

                return bestMove;
            }
        }
    }
}
