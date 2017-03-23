using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaBotImplementation
{
    public class GameState
    {
        private int[][] board;
        private int lastMovei;
        private int lastMovej;
        static Dictionary<GameState, double> probabilityTable;
        static List<GameState> gameHistory;
        public GameState(int[][] board, int lastMovei, int lastMovej)
        {
            this.board = board;
            this.lastMovei = lastMovei;
            this.lastMovej = lastMovej;
        }

        public int[][] GetBoard()
        {
            return board;
        }
        public int GetLastMoveI()
        {
            return lastMovei;
        }
        public int GetLastMoveJ()
        {
            return lastMovej;
        }
        public GameState Copy()
        {
            return new GameState(board, lastMovei, lastMovej);
        }
        public void ReadProbabilityTable()
        {
            //TODO reads probabilities from file
            probabilityTable = null;
            return;
        }
        public void UpdateProbabilityTable()
        {
            //TODO updates and re-writes probability table based on gameHistory
            return;
        }
        public List<GameState> GenerateMoves(bool player)
        {
            //TODO Generate all possible game states from current state
            return null;
        }
        public GameStatus CheckGameStatus()
        {
            //TODO
            //If I win
            return GameStatus.WIN;
            //If I lose
            return GameStatus.LOSE;
            //If game is still going
            return GameStatus.ONGOING;
        }
        public bool IsEnd()
        {
            GameStatus status = CheckGameStatus();
            return status == GameStatus.WIN || status == GameStatus.LOSE;
        }
        public double GetThreats()
        {
            //TODO Get the heuristic function of the threats
            return 0.0d;
        }
        public double GetYReduction()
        {
            //TODO Get the y reduction heuristic
            return 0.0d;
        }
        public double GetProbability()
        {
            if (probabilityTable.ContainsKey(this))
            {
                return probabilityTable[this];
            }
            else
            {
               probabilityTable.Add(this, 0.5d);
                return 0.5d;
            }
        }
        public double GetTotalHeuristic()
        {
            return GetThreats() * GetYReduction() * GetProbability();
        }
    }
}
