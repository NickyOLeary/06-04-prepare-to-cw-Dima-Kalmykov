using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWLibrary
{
    [Serializable]
    public class Pair<T, U> : IComparable
    where T:IComparable
    {
        internal T item1;
        internal U item2;

        public Pair(T item1, U item2)
        {
            this.item1 = item1;
            this.item2 = item2;
        }

        public int CompareTo(object obj)
        {
            var other = (Pair<T, U>)obj;
            return item1.CompareTo(item2);
        }

        public override string ToString() =>
            $"Item1: {item1}, Item2: {item2}";
    }
}
