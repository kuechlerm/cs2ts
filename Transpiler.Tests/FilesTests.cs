using System.IO;
using System.Linq;
using System.Reflection;
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

        [Fact]
        public void DefaultOutputTest()
        {
            // var inputTypes = Assembly.GetExecutingAssembly().GetTypes()
            //   .Where(t => t.Namespace != null && t.Namespace.EndsWith("G")
            //         && t.CustomAttributes.Contains)
            //   .ToList();

            var propsA = typeof(N.ClassA<>).GetProperties();
            var props = typeof(T.Base<>).GetProperties();

            var infs = typeof(S.ClassA).GetInterfaces();
            var infT = typeof(S.InterfaceA<>);
            var infInt = typeof(S.InterfaceA<int>);
            var blaT = typeof(Bla<>);
            var blaINt = typeof(Bla<int>);

            var path = Path.Combine("a", "\\b", "c");

            var ns = typeof(A.ClassA).Namespace;
            var p = Directory.GetCurrentDirectory();
        }

        class Bla<T>
        {

        }
    }
}