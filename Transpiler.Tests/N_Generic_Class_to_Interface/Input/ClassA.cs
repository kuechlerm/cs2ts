using System.Collections.Generic;

namespace Transpiler.Tests.N
{
    public class ClassA<T>
    {
        public T TProp { get; set; }
        public List<T> TList { get; set; }
    }
}