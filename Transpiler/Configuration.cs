using System;
using System.IO;

namespace Transpiler
{
    public class Configuration
    {
        public string TargetDirectory { get; set; }
            = Path.Combine(Directory.GetCurrentDirectory(), "CS2TS_Output");

        public bool UseNamespacesAsFolders { get; set; }
        public Func<string, string> MapNamespace { get; set; }
            = ns => ns;

        public bool PrintGeneratedFileText { get; set; }
        public bool CreateIndexFiles { get; set; }
    }
}