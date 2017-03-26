using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaBotImplementation
{
    public class GameState
    {
        public enum GameStatus { WIN, LOSE, ONGOING };

        private int[,] board;

        const int rows = 11;

        const int cols = 11;

        static Dictionary<int[,], double> probabilityTable;

        static List<GameState> gameHistory;
        public GameState(int[,] board)
        {
            this.board = board;
        }
        public override bool Equals(object obj)
        {
            var state = obj as GameState;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i,j] != state.board[i,j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            int code = 0;
            int counter = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    code += board[i, j] * (int)Math.Pow(10, counter++);
                }
            }
            return code;
        }
        public static int getRows()
        {
            return rows;
        }
        public static int getCols()
        {
            return cols;
        }

        public int[,] GetBoard()
        {
            return board;
        }
        public GameState Copy()
        {
            return new GameState(board);
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
        public static int[,] CopyBoard(int[,] board)
        {
            int[,] temp = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    temp[i, j] = board[i, j];
                }
            }
            return temp;
        }
        public List<GameState> GenerateMoves(bool player)
        {
            //TODO Generate all possible game states from current state
            int move;
            List<GameState> generatedStates = new List<GameState>();
            if (player)
            {
                move = 1; //mine
            }
            else
            {
                move = 2; //the opponent's
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == 0)
                    {
                        int[,] newBoard = CopyBoard(board);
                        newBoard[i, j] = move;
                        GameState generated = new GameState(newBoard);
                        generatedStates.Add(generated);
                    }
                }
            }
            return generatedStates;
        }
        private GameStatus CheckGameStatus()
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
            if (probabilityTable.ContainsKey(board))
            {
                return probabilityTable[board];
            }
            else
            {
               probabilityTable.Add(board, 0.5d);
                return 0.5d;
            }
        }
        public double GetTotalHeuristic()
        {
            return GetThreats() * GetYReduction() * GetProbability();
        }
    }
}
