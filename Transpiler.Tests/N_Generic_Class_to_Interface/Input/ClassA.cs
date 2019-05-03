using System.Collections.Generic;

namespace CS2TS.Tests.N
{
    public class ClassA<T>
    {
        public T TProp { get; set; }
        public List<T> TList { get; set; }
    }
}