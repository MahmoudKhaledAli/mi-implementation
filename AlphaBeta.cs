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
        private int bestMovei;
        private int bestMovej;

        public AlphaBeta()
        {
        }
        public int GetBestMoveI()
        {
            return bestMovei;
        }
        public int GetBestMoveJ()
        {
            return bestMovej;
        }
        public double Iterate(GameState state, int depth, double alpha, double beta, bool player)
        {
            if (depth == 0 || state.IsEnd())
            {
                return state.GetTotalHeuristic();
            }

            if (player == maxPlayer)
            {
                foreach (GameState child in state.GenerateMoves(player))
                {
                    double temp = Iterate(child, depth - 1, alpha, beta, !player);
                    if (temp > alpha) 
                    {
                        this.bestMovei = child.GetLastMoveI();
                        this.bestMovej = child.GetLastMoveJ();
                        alpha = temp;
                    }

                    if (beta < alpha)
                    {
                        break;
                    }

                }

                return alpha;
            }
            else
            {
                foreach (GameState child in state.GenerateMoves(player))
                {
                    beta = Math.Min(beta, Iterate(child, depth - 1, alpha, beta, !player));

                    if (beta < alpha)
                    {
                        break;
                    }
                }

                return beta;
            }
        }
    }
}
