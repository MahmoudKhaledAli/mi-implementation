using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaBotImplementation
{
    public class GameState
    {
        private int[,] board;

        private int lastMovei;

        private int lastMovej;

        const int rows = 11;

        const int cols = 11;

        public GameState(int[,] board, int lastMovei, int lastMovej)
        {
            this.board = board;
            this.lastMovei = lastMovei;
            this.lastMovej = lastMovej;
        }

        public int[,] GetBoard()
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
                        GameState generated = new GameState(newBoard, i, j);
                        generatedStates.Add(generated);
                    }
                }
            }
            return generatedStates;
        }
        private GameController.GameStatus CheckGameStatus()
        {
            //TODO
            //If I win
            return GameController.GameStatus.WIN;
            //If I lose
            return GameController.GameStatus.LOSE;
            //If game is still going
            return GameController.GameStatus.ONGOING;
        }
        public bool IsEnd()
        {
            GameController.GameStatus status = CheckGameStatus();
            return status == GameController.GameStatus.WIN || status == GameController.GameStatus.LOSE;
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
        public double GetProbability(Dictionary<int[,], double> probabilityTable)
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
        public double GetTotalHeuristic(Dictionary<int[,], double> probabilityTable)
        {
            return GetThreats() * GetYReduction() * GetProbability(probabilityTable);
        }
    }
}
