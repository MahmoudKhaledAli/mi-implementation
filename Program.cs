using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaBotImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] x = new int[11, 11];
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    x[i, j] = 0;
                }
            }
            x[0, 0] = 1;
            GameState state = new GameState(x, 0, 0);
            List<GameState> list = state.GenerateMoves(false);
        }
    }
}
