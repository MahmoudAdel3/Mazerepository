using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Files
    {
        public static void readfromfile(out int[,] result, String path)
        {
            String input = File.ReadAllText(path);
            int i = 0, j = 0;
            result = new int[25, 25];
            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
            

        }
    }
}
