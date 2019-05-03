using System.Collections.Generic;

namespace CS2TS.Tests
{
    public class TestFileWriter : IFileWriter
    {
        public List<string> CreatedDirectoryPaths { get; private set; } = new List<string>();
        public Dictionary<string, List<string>> CreatedFiles { get; private set; }
            = new Dictionary<string, List<string>>();

        public void RecreateDirectory(string path)
        {
            this.CreatedDirectoryPaths.Add(path);
        }

        public void CreateFile(string path, List<string> lines)
        {
            this.CreatedFiles.Add(path, lines);
        }
    }
}