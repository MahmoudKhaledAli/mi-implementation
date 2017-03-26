using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaBotImplementation
{
    public class Hashing
    {
        static int[,] table;
        static Hashing()
        {
            Random rnd = new Random();
            table = new int[121, 2];
            for (int i = 0; i < 121; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    table[i, j] = rnd.Next(1, 121 * 2);
                }
            }
        }
        public static int Hash(GameState state)
        {
            int h = 0;
            int[,] board = state.GetBoard();
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (board[i, j] != 0)
                    {
                        h = h ^ table[i * 11 + j, board[i, j] - 1];
                    }
                }
            }
            return h;
        }
    }
}
