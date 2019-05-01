using System.Collections.Generic;

namespace Transpiler.Tests.O
{
    public class ClassA<T, U>
    {
        public T TProp { get; set; }
        public U UProp { get; set; }
    }
}