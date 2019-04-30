using System.Collections.Generic;

namespace Transpiler
{
    public interface IFileWriter
    {
        void CreateDirectory(string path);
        void CreateFile(string path, List<string> lines);
    }
}