using System.IO;
using System.Linq;
using Xunit;

namespace Transpiler.Tests
{
    public class FilesTests
    {
        [Fact(Skip = "Manual")]
        public void GetTsFiles()
        {
            var path = @"C:\Dev\Tools\cs2ts\Transpiler.Tests\E_Subfolders\Output";

            var tsFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Select(x => new TsFile
                {
                    Name = Path.GetFileName(x),
                    Directory = x.Substring(path.Length, x.Length - path.Length - Path.GetFileName(x).Length),
                    Lines = File.ReadAllLines(x).ToList()
                });

        }

        [Fact(Skip = "Manual")]
        public void DefaultOutputTest()
        {
            var p = Directory.GetCurrentDirectory();
        }
    }
}