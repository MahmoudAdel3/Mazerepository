using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class PointDirection : IEquatable<PointDirection>
    {
        public int point;
        public char direction;
        public bool Equals(PointDirection other)
        {
            // Would still want to check for null etc. first.
            return this.point == other.point &&
                   this.direction == other.direction;
                   
        }
    }

}
