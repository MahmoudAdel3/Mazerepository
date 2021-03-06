﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class SequentialMaze
    {
        
        private int[,] matrix;
        private Directions directions = new Directions();
        private Dictionary<PointDirection, PointDirection> map = new Dictionary<PointDirection, PointDirection>(new Compartor());
        int m, n;

        public SequentialMaze(int[,] matrix,int m,int n)
        {
            this.matrix = matrix;
            this.m = m;
            this.n = n;
        }

        public void printMaze()
        {
            
            for (int i = 0; i < m; i++)
            {

                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + " ");

                }
                Console.Write("\n");
            }

        }

        public void solveMaze(int source, int dest)
        {
            List<int> points = findPath(source,dest);


            for (int i = 0; i < points.Count(); i++)
            {
                int Row = points[i] / n;
                int Col = points[i] % n;
                matrix[Row, Col] = States.hoverCar;
            }
        }

        private Boolean inMatrix(int point)
        {
            return point <= (m * n) - 1 && point >= 0 ? true : false;
        }
        private Boolean isOpen(int point)
        {
            int Row = point / n;
            int Col = point % n;
            if (matrix[Row, Col] != 0)
                return true;
            return false;
        }
        private Boolean notLast(int point, int last)
        {
            if ((point % n) == 0 && (point - last) == 1)
                return false;
            if (((point + 1) % n) == 0 && (point - last) == -1 && point != 0)
                return false;
            return true;
        }
        public List<int> findPath(int source, int dest)
        {
            PointDirection p = new PointDirection();
            p.point = source; p.direction = Directions.north;
            Queue<PointDirection> Visited = new Queue<PointDirection>();
            List<PointDirection> VisitedNodes = new List<PointDirection>();
            PointDirection destination = new PointDirection();
            VisitedNodes.Add(p);
            Visited.Enqueue(p);
         for(; Visited.Count > 0;)
            {

                PointDirection current = Visited.Dequeue();
                PointDirection nextfirst = new PointDirection();
                PointDirection nextsecond = new PointDirection();
                
                if (current.point == dest)
                {
                    destination.point = dest;
                    destination.direction = current.direction;
                    break;
                }
                    
                int[] moves = directions.move(current.direction, n); ;
                int firstMove = moves[0];
                int secondMove = moves[1];
                if (firstMove > 0)
                    nextfirst.direction = Directions.east;
                else
                    nextfirst.direction = Directions.west;
                nextfirst.point = current.point + firstMove;
                if (inMatrix(current.point + firstMove) && isOpen(current.point + firstMove) && !VisitedNodes.Contains(nextfirst) && notLast(current.point + firstMove, current.point))
                {
                    nextfirst.point = current.point + firstMove;
                    Visited.Enqueue(nextfirst);
                    VisitedNodes.Add(nextfirst);
                    map.Add(nextfirst, current);

                }
                if (secondMove > 0)
                    nextsecond.direction = Directions.south;
                else
                    nextsecond.direction = Directions.north;
                nextsecond.point = current.point + secondMove;
                if (inMatrix(current.point + secondMove) && isOpen(current.point + secondMove) && !VisitedNodes.Contains(nextsecond))
                {
                    nextsecond.point = current.point + secondMove;
                    Visited.Enqueue(nextsecond);
                    VisitedNodes.Add(nextsecond);
                    map.Add(nextsecond, current);
                }


            }
            List<int> points = new List<int>();
            if (destination!=null)
            {
                PointDirection key = destination;
                if (map.ContainsKey(key))
                {


                    points.Add(destination.point);
                    while (map[key] != p)
                    {
                        points.Add(map[key].point);
                        key = map[key];
                    }
                    points.Add(map[key].point);
                }

            }

            return points;
        }


    }
}
