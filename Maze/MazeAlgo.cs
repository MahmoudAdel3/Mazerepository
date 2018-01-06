using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class MazeAlgo
    {

        public int[,] matrix;

        private int[] hoverCarIndex = new int[2];
        private int[] outIndex = new int[2];
        private int m, n;
        //     public char currentDirection = Directions.north;
        List<int> visited = new List<int>();
        //   Directions directions = new Directions();

        // more ones to give it more priority
        private int[] mazeTemp = new int[] { States.free, States.free, States.free, States.free };


        public MazeAlgo(int m, int n)
        {
            this.m = m;
            this.n = n;
            generateMaze();
        }

        private void generateMaze()
        {

            int outPriority = 7;
            matrix = new int[m, n];

            bool flag = false;

            Random random = new Random();

            for (int i = 0; i < m; i++)
            {
                int j = 0;
                if (!flag && random.Next(outPriority) == 0)
                {
                    matrix[i, 0] = States._out;
                    outIndex[0] = i;
                    outIndex[1] = 0;
                    flag = true;
                    j = 1;

                }
                for (; j < n; j++)
                {
                    if (!flag && (i == 0 || i == (m - 1)) && random.Next(outPriority) == 0)
                    {
                        matrix[i, j] = States._out;
                        outIndex[0] = i;
                        outIndex[1] = j;
                        flag = true;
                    }
                    else
                    {
                        // fill the maze with '1' or '0' randomlly
                        matrix[i, j] = mazeTemp[random.Next(mazeTemp.Length)];
                    }

                }
                if (!flag && random.Next(outPriority) == 0)
                {
                    matrix[i, n - 1] = States._out;
                    outIndex[0] = i;
                    outIndex[1] = n - 1;
                    flag = true;
                }
            }
            if (!flag)
            {
                matrix[m - 1, n - 1] = States._out;
                outIndex[0] = m - 1;
                outIndex[1] = n - 1;
            }

            // put hoverCar in the maze 
            hoverCarIndex[0] = random.Next(0, m);
            hoverCarIndex[1] = random.Next(0, n);

            matrix[hoverCarIndex[0], hoverCarIndex[1]] = States.hoverCar;

        }
    }





}

