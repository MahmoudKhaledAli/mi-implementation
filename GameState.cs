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

        const double probfactor = 0.1d;

        const int rows = 11;

        const int cols = 1;

        const int ySize = 2 * rows - 1;

        static Dictionary<string, double> probabilityTable;

        static List<GameState> gameHistory;
        private struct Position
        {
            public int i;
            public int j;
            public Position(int i, int j) : this()
            {
                this.i = i;
                this.j = j;
            }
        }
        private struct Ones
        {
            public Position pos;
            public List<Position> AdjList;
            public Ones(Position p, List<Position> l)
            {
                pos = p;
                AdjList = l;
            }
        }
        public GameState()
        {
            this.board = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = 0;
                }
            }
        }
        public GameState(int[,] board)
        {
            this.board = CopyBoard(board);
        }
        public static int GetRows()
        {
            return rows;
        }
        public static int GetCols()
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
        public static void ReadProbabilityTable()
        {
            //TODO reads probabilities from file
            probabilityTable = new Dictionary<string, double>();
            Deserialize();
            return;
        }
        private string GetString()
        {
            string boardState = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == 0)
                    {
                        boardState += "0";
                    }
                    else if (board[i, j] == 1)
                    {
                        boardState += "1";
                    }
                    else
                    {
                        boardState += "2";
                    }
                }
            }
            return boardState;
        }
        public static void InitHistory()
        {
            gameHistory = new List<GameState>();
        }
        public void AddToHistory()
        {
            gameHistory.Add(new GameState(board));
        }
         public static void UpdateProbabilityTable(GameStatus status)
        {
            bool turn = false;
            if (status == GameStatus.WIN)
            {
                turn = true;
                for (int i = gameHistory.Count() - 1; i > -1; i--)
                {
                    if (turn)
                    {
                        if (probabilityTable.ContainsKey(gameHistory[i].GetString()))
                        {
                            if (probabilityTable[gameHistory[i].GetString()] < 1)
                            {
                                probabilityTable[gameHistory[i].GetString()] += ((i * probfactor) / gameHistory.Count());
                                if (probabilityTable[gameHistory[i].GetString()] > 1)
                                    probabilityTable[gameHistory[i].GetString()] = 1;
                            }
                        }
                        else
                        {
                            probabilityTable.Add(gameHistory[i].GetString(), 0.5 + ((i * probfactor) / gameHistory.Count()));
                        }
                        turn = false;
                    }
                    else
                    {
                        if (probabilityTable.ContainsKey(gameHistory[i].Transpose().GetString()))
                        {
                            if (probabilityTable[gameHistory[i].Transpose().GetString()] > 0)
                            {
                                probabilityTable[gameHistory[i].Transpose().GetString()] -= ((i * probfactor) / gameHistory.Count());
                                if (probabilityTable[gameHistory[i].Transpose().GetString()] < 0)
                                    probabilityTable[gameHistory[i].Transpose().GetString()] = 0;
                            }
                        }
                        else
                        {
                            probabilityTable.Add(gameHistory[i].Transpose().GetString(), 0.5 - ((i * probfactor) / gameHistory.Count()));
                        }
                        turn = true;
                    }
                }
            }
            else
            {
                for (int i = gameHistory.Count() - 1; i > -1; i--)
                {
                    if (turn)
                    {
                        if (probabilityTable.ContainsKey(gameHistory[i].GetString()))
                        {
                            if (probabilityTable[gameHistory[i].GetString()] > 0)
                            {
                                probabilityTable[gameHistory[i].GetString()] -= ((i * probfactor) / gameHistory.Count());
                                if (probabilityTable[gameHistory[i].GetString()] < 0)
                                    probabilityTable[gameHistory[i].GetString()] = 0;
                            }
                        }
                        else
                        {
                            probabilityTable.Add(gameHistory[i].GetString(), 0.5 - ((i * probfactor) / gameHistory.Count()));
                        }
                        turn = false;
                    }
                    else
                    {
                        if (probabilityTable.ContainsKey(gameHistory[i].Transpose().GetString()))
                        {
                            if (probabilityTable[gameHistory[i].Transpose().GetString()] < 1)
                            {
                                probabilityTable[gameHistory[i].Transpose().GetString()] += ((i * probfactor) / gameHistory.Count());
                                if (probabilityTable[gameHistory[i].Transpose().GetString()] > 1)
                                    probabilityTable[gameHistory[i].Transpose().GetString()] = 1;
                            }
                        }
                        else
                        {
                            probabilityTable.Add(gameHistory[i].Transpose().GetString(), 0.5 + ((i * probfactor) / gameHistory.Count()));
                        }
                        turn = true;
                    }
                }
            }

            Serialize();

            return;
        }
        private static int[,] CopyBoard(int[,] board)
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
        public List<GameState> GenerateMoves(bool player, bool firstMove)
        {
            //TODO Generate all possible game states from current state
            int move;
            List<GameState> generatedStates = new List<GameState>();

            if (firstMove)
            {
                generatedStates.Add(Transpose());
            }

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
            generatedStates.Shuffle();
            return generatedStates;
        }
        private bool InRange(int i, int j)
        {
            return i >= 0 && i < rows && j >= 0 && j < cols;
        }
        private List<Position> GetAdjacent(Position pos)
        {
            List<Position> adjacents = new List<Position>();
            if (InRange(pos.i + 1, pos.j))
            {
                adjacents.Add(new Position(pos.i + 1, pos.j));
            }

            if (InRange(pos.i - 1, pos.j))
            {
                adjacents.Add(new Position(pos.i - 1, pos.j));
            }

            if (InRange(pos.i, pos.j + 1))
            {
                adjacents.Add(new Position(pos.i, pos.j + 1));
            }

            if (InRange(pos.i, pos.j - 1))
            {
                adjacents.Add(new Position(pos.i, pos.j - 1));
            }

            if (InRange(pos.i - 1, pos.j + 1))
            {
                adjacents.Add(new Position(pos.i - 1, pos.j + 1));
            }

            if (InRange(pos.i + 1, pos.j - 1))
            {
                adjacents.Add(new Position(pos.i + 1, pos.j - 1));
            }

            return adjacents;
        }
        public GameStatus CheckGameStatus()
        {
            //TODO
            //If I win
            Queue<Position> myPositions = new Queue<Position>();
            Position myPosition;
            int[,] dummyBoard = CopyBoard(board);

            for (int i = 0; i < rows; i++)
            {
                if (dummyBoard[i, 0] == 1)
                {
                    myPositions.Enqueue(new Position(i, 0));
                    dummyBoard[i, 0] = 0;
                }
            }

            while (myPositions.Count != 0)
            {
                myPosition = myPositions.Dequeue();
                foreach (Position pos in GetAdjacent(myPosition))
                {
                    if (dummyBoard[pos.i, pos.j] == 1)
                    {
                        if (pos.j == cols - 1)
                        {
                            return GameStatus.WIN;
                        }
                        else
                        {
                            myPositions.Enqueue(pos);
                            dummyBoard[pos.i, pos.j] = 0;
                        }
                    }
                }
            }

            dummyBoard = CopyBoard(board);
            Queue<Position> oppPositions = new Queue<Position>();
            Position oppPosition;

            for (int i = 0; i < cols; i++)
            {
                if (dummyBoard[0, i] == 2)
                {
                    oppPositions.Enqueue(new Position(0, i));
                    dummyBoard[0, i] = 0;
                }
            }

            while (oppPositions.Count != 0)
            {
                oppPosition = oppPositions.Dequeue();
                foreach (Position pos in GetAdjacent(oppPosition))
                {
                    if (dummyBoard[pos.i, pos.j] == 2)
                    {
                        if (pos.i == rows - 1)
                        {
                            return GameStatus.LOSE;
                        }
                        else
                        {
                            oppPositions.Enqueue(pos);
                            dummyBoard[pos.i, pos.j] = 0;
                        }
                    }
                }
            }

            //If game is still going
            return GameStatus.ONGOING;
        }
        public bool IsEnd()
        {
            GameStatus status = CheckGameStatus();
            return status == GameStatus.WIN || status == GameStatus.LOSE;
        }
        public double GetTurnNo()
        {
            double count = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        List<Ones> GetACopy()
        {
            List<Ones> Copy = new List<Ones>();
            for (int i = 0; i < rows; i++)
            {
                
                for (int j = 0; j < cols; j++)
                {

                    if (board[i, j] == 1)
                    {
                        Position tempPos = new Position(i, j);
                        List<Position> tempList = new List<Position>();
                        tempList = GetAdjacent(tempPos);
                        Ones tempOne = new Ones(tempPos, tempList);
                       

                        Copy.Add(tempOne);

                    }
                }
            }
            return Copy;
        }
        public double GetMyThreats()
        {
            //TODO Get the heuristic function of the threats
            int rows = board.GetLength(0);
            int col = board.GetLength(1);
            List<Ones> myList = new List<Ones>();
            
            List<Position> ThreatsPos = new List<Position>();
            double Threats = 0.0;

            for (int i=0;i<rows;i++)
            {
                for(int j=0;j<col;j++)
                {
                    
                    if(board[i,j]==1)
                    {
                        Position tempPos = new Position(i, j);
                        List<Position> tempList = new List<Position>();
                        tempList = GetAdjacent(tempPos);
                        Ones tempOne = new Ones(tempPos, tempList);
                        myList.Add(tempOne);
                        
                        

                    }
                }
            }


            for (int i = 0; i < myList.Count; i++)
            {
                //List<Ones> Copy = myList;
                List<Ones> Copy = GetACopy();
                Position currentPos = myList[i].pos;
                List<Position> currentList = myList[i].AdjList;
                Position checkingPos1 = new Position(0, 0);
                Position checkingPos2 = checkingPos1;
                bool firstCheck = false, secondCheck = false;
                bool exists = false;

                int j = i + 1;
                int index = i + 1;

                //check if this one is connected with any other one or not
                while (j != Copy.Count)
                {
                    exists = IsInThisList(currentPos, Copy[j].AdjList);

                    if (exists)
                    {
                        Copy.RemoveAt(j);
                        j++;
                        j--;
                    }
                    else
                        j++;
                }

                if (Copy.Count != 1) // donc fih lesa 3l a2al one wahda msh connected beya  3shan law =1 yb2a ana bas  
                {
                    for (int l = index; l < Copy.Count; l++)
                    {
                        Position CurrentPosInList;
                        bool existed;
                        bool firstTime = true;
                        for (int k = 0; k < currentList.Count; k++)
                        {
                            CurrentPosInList = currentList[k];
                            existed = IsInThisList(CurrentPosInList, Copy[l].AdjList);
                            if (existed)
                            {
                                if (firstTime)
                                {
                                    checkingPos1 = CurrentPosInList;
                                        ThreatsPos.Add(checkingPos1);
                                        firstCheck = true;
                                   
                                    
                                    firstTime = false;
                                }
                                else
                                {
                                    //second time and the last one
                                    checkingPos2 = CurrentPosInList;
                                   
                                        ThreatsPos.Add(checkingPos1);
                                        secondCheck = true;
                                    
                                }
                            }
                            if (firstCheck && secondCheck) //donc weslet lel pair el common ma ben 2 ones
                            {
                                if (board[checkingPos1.i, checkingPos1.j] == 0)
                                {
                                    
                                        if (board[checkingPos2.i, checkingPos2.j] == 0)
                                            Threats += 1;
                                        else if (board[checkingPos2.i, checkingPos2.j] == 1)
                                            Threats += 1;
                                    
                                }
                                else if (board[checkingPos1.i, checkingPos1.j] == 1)
                                {
                                   
                                     if (board[checkingPos2.i, checkingPos2.j] == 0)
                                            Threats += 1;
                                    

                                }

                                firstCheck = false;
                                secondCheck = false;
                                firstTime = true;

                            }

                        }

                    }

                }
            }

            return Threats;
        }

        public double GetHisThreats()
        {
            
            return this.Transpose().GetMyThreats();
        }

        public double GetThreats()
        {
            double MyThreats = GetMyThreats();
            double HisThreats = GetHisThreats();
            return (1 + MyThreats) / (1 + HisThreats);
        }
        private bool IsInThisList(Position p, List<Position> l)
        {
            for(int k=0;k<l.Count;k++)
            {
                if ((l[k].i==p.i) && (l[k].j==p.j))
                    return true;
            }
            return false;
        }
        public double GetYReduction()
        {
            //TODO Get the y reduction heuristic
            return MicroReduction(ConvertToY(), ySize);
        }
        public double GetOtherYReduction()
        {
            return MicroReduction(Transpose().ConvertToY(), ySize);
        }
        private double GetProbability()
        {
            if (probabilityTable.ContainsKey(this.GetString()))
            {
                return probabilityTable[this.GetString()];
            }
            else
            {
                return 0.5d;
            }
        }
        public double GetTotalHeuristic()
        {
            if (CheckGameStatus() == GameStatus.WIN)
            {
                return 100000000.0d / GetTurnNo();
            }
            if (CheckGameStatus() == GameStatus.LOSE)
            {
                return -100000000.0d;
            }
            return (GetThreats() * GetYReduction() * GetProbability()) / GetTurnNo();
        }
        private static void Deserialize()
        {
            try
            {
                var f_fileStream = File.OpenRead(@"dictionarySerialized.xml");
                var f_binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                probabilityTable = (Dictionary<string, double>)f_binaryFormatter.Deserialize(f_fileStream);
                f_fileStream.Close();
            }
            catch (Exception ex)
            {
                ;
            }
        }
        private static void Serialize()
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
        private double[][] CreateNewY(int size)
        {
            double[][] gameOfY = new double[size][];
            for (int i = 0; i < size; i++)
            {
                gameOfY[i] = new double[i + 1];
            }
            return gameOfY;
        }
        private double[][] ConvertToY()
        {
            double[][] gameOfY = CreateNewY(ySize);

            for (int i = 0; i < rows - 1; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    gameOfY[i][j] = 0;
                }
            }
            for (int i = rows - 1; i < ySize; i++)
            {
                for (int j = 0; j < i - rows + 1; j++)
                {
                    gameOfY[i][j] = 1;
                }
                for (int j = i - rows + 1; j < i + 1; j++)
                {
                    switch (board[i - rows + 1, j - i + rows - 1])
                    {
                        case 0:
                            gameOfY[i][j] = 0.5d;
                            break;
                        case 1:
                            gameOfY[i][j] = 1.0d;
                            break;
                        case 2:
                            gameOfY[i][j] = 0.0d;
                            break;
                        default:
                            gameOfY[i][j] = 0.5d;
                            break;
                    }
                }
            }

            return gameOfY;
        }
        private double CalculateReductionProbability(double p1, double p2, double p3)
        {
            return p1 * p2 + p1 * p3 + p2 * p3 - 2 * p1 * p2 * p3;
        }
        private double MicroReduction(double[][] gameOfY, int size)
        {
            if (size == 1)
            {
                return gameOfY[0][0];
            }

            double[][] newGameOfY = CreateNewY(size - 1);

            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    newGameOfY[i][j] = CalculateReductionProbability(gameOfY[i][j], gameOfY[i + 1][j + 1], gameOfY[i + 1][j]);
                }
            }

            return MicroReduction(newGameOfY, size - 1);
        }
        public void ApplyMove(Move move, bool player)
        {
            if (move.GetMoveI() == -1 && move.GetMoveJ() == -1)
            {
                board = Transpose().board;
            }
            int newHex = player ? 1 : 2;
            board[move.GetMoveI(), move.GetMoveJ()] = newHex;
        }
        public GameState Transpose()
        {
            GameState tranposed = new GameState();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    switch (board[j, i])
                    {
                        case 1:
                            tranposed.board[i, j] = 2;
                            break;
                        case 2:
                            tranposed.board[i, j] = 1;
                            break;
                        default:
                            tranposed.board[i, j] = 0;
                            break;
                    }
                }
            }
            return tranposed;
        }
        public int GetEmptyCount()
        {
            int noOfMoves = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == 0)
                    {
                        noOfMoves++;
                    }
                }
            }
            return noOfMoves;
        }
    }
}
