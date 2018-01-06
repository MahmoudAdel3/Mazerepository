using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Directions
    {

        public static readonly char north = 'N';
        public static readonly char east = 'E';
        public static readonly char west = 'W';
        public static readonly char south = 'S';

        public int[] move(char c, int col)
        {
            int[] moves = new int[2];
            if (c == north)
            {
                moves[0] = 1;
                moves[1] = -col;
            }
            else if (c == south)
            {
                moves[0] = -1;
                moves[1] = col;
            }
            else if (c == east)
            {
                moves[0] = 1;
                moves[1] = col;
            }
            else
            {
                moves[0] = -1;
                moves[1] = -col;
            }


            return moves;

        }
    }
}
