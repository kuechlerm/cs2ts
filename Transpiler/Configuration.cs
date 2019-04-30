using System.IO;

namespace Transpiler
{
    public class Configuration
    {
        public string TargetDirectory { get; set; }
            = Path.Combine(Directory.GetCurrentDirectory(), "CS2TS_Output");
        public bool UseNamespacesAsFolders { get; set; }
        public bool PrintGeneratedFileText { get; set; }
    }
}