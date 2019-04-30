using System.Collections.Generic;

namespace Transpiler
{
    public class TsFile
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public List<string> Lines { get; set; }
    }
}