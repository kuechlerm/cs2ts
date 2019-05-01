using System;
using System.Collections.Generic;

namespace Transpiler
{
    public class TsType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Directory { get; set; }
        public List<string> GenericArguments { get; set; }
        public Type Type { get; set; }
    }
}