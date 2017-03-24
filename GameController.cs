using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaBotImplementation
{
    class GameController
    {
        public enum GameStatus { WIN, LOSE, ONGOING };

        private Dictionary<int[,], double> probabilityTable;

        private List<GameState> gameHistory;

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

        public Dictionary<int[,], double> GetProbabilityTable()
        {
            return probabilityTable;
        }
    }
}
