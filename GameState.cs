using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HexaBotImplementation
{
   


    public class GameState
    {
        public enum GameStatus { WIN, LOSE, ONGOING };

        private int[,] board;

        const int rows = 11;

        const int cols = 11;

        static Dictionary<GameState, double> probabilityTable;

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
            return Hashing.Hash(this);
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
            probabilityTable = new Dictionary<GameState, double>();
            Deserialize();
            return;
        }
        public void UpdateProbabilityTable()
        {
            for(int i=0;i< gameHistory.Count(); i++)
            {
                if(probabilityTable.ContainsKey(gameHistory[i]))
                {
                    probabilityTable[gameHistory[i]] = gameHistory[i].GetProbability() + (((i+1) * 0.1) / gameHistory.Count());
                }
                else
                {
                    probabilityTable.Add(gameHistory[i], ((i + 1) * 0.1) / gameHistory.Count());
                }
            }
            Serialize();
            
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
        private void Deserialize()
        {
            try
            {
                var f_fileStream = File.OpenRead(@"dictionarySerialized.xml");
                var f_binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                probabilityTable = (Dictionary<GameState,double>)f_binaryFormatter.Deserialize(f_fileStream);
                f_fileStream.Close();
            }
            catch (Exception ex)
            {
                ;
            }
        }
        private void Serialize()
        {
            try
            {
                var f_fileStream = new FileStream(@"dictionarySerialized.xml", FileMode.Create, FileAccess.Write);
                var f_binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                f_binaryFormatter.Serialize(f_fileStream, probabilityTable);
                f_fileStream.Close();
            }
            catch (Exception ex)
            {
                ;
            }
        }
    }
}
