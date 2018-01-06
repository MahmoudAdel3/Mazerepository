using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze
{
    class Compartor : IEqualityComparer<PointDirection>
    {

        #region IEqualityComparer<Customer> Members

        public bool Equals(PointDirection x, PointDirection y)
        {
            return ((x.point == y.point) & (x.direction == y.direction));
        }

        public int GetHashCode(PointDirection obj)
        {
            string combined = obj.point + "|" + obj.direction.ToString();
            return (combined.GetHashCode());
        }

        #endregion

    }
}
