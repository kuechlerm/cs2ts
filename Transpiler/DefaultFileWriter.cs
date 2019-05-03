using System.Collections.Generic;
using System.IO;

namespace CS2TS
{
    public class DefaultFileWriter : IFileWriter
    {
        public void RecreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }

        public void CreateFile(string path, List<string> lines)
        {
            new FileInfo(path).Directory.Create();
            File.WriteAllLines(path, lines);
        }
    }
}