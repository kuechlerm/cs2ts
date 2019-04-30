using System.Collections.Generic;
using System.IO;

namespace Transpiler
{
    public class DefaultFileWriter : IFileWriter
    {
        public void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }

        public void CreateFile(string path, List<string> lines)
        {
            File.WriteAllLines(path, lines);
        }
    }
}