using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class ParallelMaze
    {
        private int[,] matrix;
        private Directions directions = new Directions();
        private Dictionary<PointDirection,PointDirection> map = new Dictionary<PointDirection,PointDirection>(new Compartor());
        private Dictionary<PointDirection, PointDirection> first = new Dictionary<PointDirection,PointDirection>(new Compartor());
        private Dictionary<PointDirection,PointDirection> second =new Dictionary<PointDirection,PointDirection>(new Compartor()) ;
        int m, n;

        public ParallelMaze(int[,] matrix, int m, int n)
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
               
                Parallel.For(0,points.Count,i=>{
                     int Row = points[i] / n;
                    int Col = points[i] % n;;
                    matrix[Row, Col] = States.hoverCar;

                });
            
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
            List<int> points = new List<int>();
            PointDirection p = new PointDirection();
            var firsttokenSource = new System.Threading.CancellationTokenSource();
            var token1 = firsttokenSource.Token;
            var secondtokenSource = new System.Threading.CancellationTokenSource();
            var token2 = secondtokenSource.Token;
            PointDirection nextfirst = new PointDirection();
            PointDirection nextsecond = new PointDirection();
            Task <PointDirection>firsttask = null;
            Task<PointDirection> secondtask = null;
            p.point = source; p.direction = Directions.north;
            if (p.point == dest)
            {
                points.Add(dest);
                return points;
            }
           
            int[] moves = directions.move(p.direction, n); 
            int firstMove = moves[0];
            int secondMove = moves[1];
           
            if (firstMove > 0)
                nextfirst.direction = Directions.east;
            else
                nextfirst.direction = Directions.west;
            if (inMatrix(p.point + firstMove) && isOpen(p.point + firstMove) && notLast(p.point + firstMove, p.point))
            {
               
                
                nextfirst.point = p.point + firstMove;
                first.Add(nextfirst, p);
                 firsttask = new Task<PointDirection>(()=>path(nextfirst,dest,first),token1);
                 firsttask.Start();
                
            }
            if (secondMove > 0)
                nextsecond.direction = Directions.south;
            else
                nextsecond.direction = Directions.north;
            if (inMatrix(p.point + secondMove) && isOpen(p.point + secondMove))
            {
                
                nextsecond.point = p.point + secondMove;
                second.Add(nextsecond, p);
                secondtask = new Task<PointDirection>(()=>path(nextsecond,dest,second),token2);
                secondtask.Start();
            }
            int index = 2;
            if (firsttask != null && secondtask != null)
            { 
                 index = Task.WaitAny(secondtask, firsttask);
                 if (index < 2)
                 {
                     if (index == 1)
                     {
                         secondtokenSource.Cancel();
                         if (secondtask.Result!=null)
                         {

                             PointDirection d = secondtask.Result;
                             points.Add(d.point);
                             while (second[d] != p)
                             {
                                 points.Add(second[d].point);
                                 d = second[d];
                             }
                             points.Add(second[d].point);
                         }
                         
                     }
                     else if (index == 0)
                     {
                         firsttokenSource.Cancel();
                         if (firsttask.Result != null)
                         {

                             PointDirection d = firsttask.Result;
                             points.Add(d.point);
                             while (first[d] != p)
                             {
                                 points.Add(first[d].point);
                                 d = first[d];
                             }
                             points.Add(first[d].point);
                         }
                     }
                 }
                
            }
            else if (firsttask != null && secondtask == null)
            {
                firsttask.Wait();
                Task result= firsttask.ContinueWith(s =>
                {
                    if (firsttask.Result != null)
                    {

                        PointDirection d = firsttask.Result;
                        points.Add(d.point);
                        while (first[d] != p)
                        {
                            points.Add(first[d].point);
                            d = first[d];
                        }
                        points.Add(first[d].point);
                    }
                });
                result.Wait();
            }
            else
            { 
                secondtask.Wait();
                if (secondtask.Result != null)
                {

                    PointDirection d = secondtask.Result;
                    points.Add(d.point);
                    while (second[d] != p)
                    {
                        points.Add(second[d].point);
                        d = second[d];
                    }
                    points.Add(second[d].point);
                }
            }

            return points;
        }



        public PointDirection path(PointDirection source, int dest, Dictionary<PointDirection, PointDirection> map)
        {
            PointDirection p = new PointDirection();

            Queue<PointDirection> Visited = new Queue<PointDirection>();
            List<PointDirection> VisitedNodes = new List<PointDirection>();
            PointDirection destination = new PointDirection();
            VisitedNodes.Add(source);
            Visited.Enqueue(source);
            PointDirection prev = new PointDirection();
            PointDirection current = new PointDirection();
            for (; Visited.Count > 0; )
            {
               
                prev = current;
                current = Visited.Dequeue();
                PointDirection nextfirst = new PointDirection();
                PointDirection nextsecond = new PointDirection();

                if (current.point == dest)
                {
                    
                    //map.Add(current,prev);
                    return current;
                    
                    
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
            return null;
        }
        //private void path(PointDirection source, int dest,Dictionary<int,int>map)
        //{
        //    Queue<PointDirection> Visited = new Queue<PointDirection>();
        //    List<int> VisitedNodes = new List<int>();
        //    VisitedNodes.Add(source.point);
        //    Visited.Enqueue(source);
        //    for (; Visited.Count > 0;)
        //    {

        //        PointDirection current = Visited.Dequeue();
        //        PointDirection nextfirst = new PointDirection();
        //        PointDirection nextsecond = new PointDirection();

        //        if (current.point == dest)
        //            break;
        //        int[] moves = directions.move(current.direction, n); ;
        //        int firstMove = moves[0];
        //        int secondMove = moves[1];
        //        if (firstMove > 0)
        //            nextfirst.direction = Directions.east;
        //        else
        //            nextfirst.direction = Directions.west;
        //        if (inMatrix(current.point + firstMove) && isOpen(current.point + firstMove) && !VisitedNodes.Contains(current.point + firstMove) && notLast(current.point + firstMove, current.point))
        //        {
        //            nextfirst.point = current.point + firstMove;
        //            Visited.Enqueue(nextfirst);
        //            VisitedNodes.Add(nextfirst.point);
        //            map.Add(nextfirst.point, current.point);

        //        }
        //        if (secondMove > 0)
        //            nextsecond.direction = Directions.south;
        //        else
        //            nextsecond.direction = Directions.north;
        //        if (inMatrix(current.point + secondMove) && isOpen(current.point + secondMove) && !VisitedNodes.Contains(current.point + secondMove))
        //        {
        //            nextsecond.point = current.point + secondMove;
        //            Visited.Enqueue(nextsecond);
        //            VisitedNodes.Add(nextsecond.point);
        //            map.Add(nextsecond.point, current.point);
        //        }


        //    }
            
        //}
    }
}
