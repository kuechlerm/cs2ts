using System.Collections.Generic;

namespace CS2TS
{
    public interface IFileWriter
    {
        void RecreateDirectory(string path);
        void CreateFile(string path, List<string> lines);
    }
}