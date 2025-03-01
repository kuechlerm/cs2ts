using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace CS2TS.Tests
{
    public class InputOutputTests
    {
        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [InlineData("C")]
        [InlineData("D")]
        [InlineData("E")]
        [InlineData("F")]
        [InlineData("G")]
        [InlineData("H")]
        [InlineData("I")]
        [InlineData("J")]
        [InlineData("K")]
        [InlineData("M")]
        [InlineData("N")]
        [InlineData("O")]
        [InlineData("P")]
        [InlineData("Q")]
        [InlineData("R")]
        [InlineData("S")]
        [InlineData("T")]
        [InlineData("U")]
        [InlineData("V")]
        [InlineData("W")]
        public void Matches(string ns)
        {
            var fullNamespace = "CS2TS.Tests." + ns;

            // Create Transpiler
            var configType = Assembly.GetExecutingAssembly().GetTypes()
                .SingleOrDefault(t => t.Namespace == fullNamespace && t.Name == "Config");

            var config = configType == null
                ? new Configuration()
                : Activator.CreateInstance(configType) as Configuration;

            var fileWriter = new TestFileWriter();
            var transpiler = new Transpiler(config, fileWriter);

            // Get expected output
            var testDirs = Directory.GetDirectories(@"C:\Dev\Tools\cs2ts\Transpiler.Tests\");
            var outputPath = testDirs
                .SingleOrDefault(x => x.Split("\\").Last().StartsWith(ns + "_"))
                + "\\Output";

            var inputTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null
                    && (t.Namespace == fullNamespace || t.Namespace.StartsWith(fullNamespace + ".")) && !t.Name.StartsWith("Config")
                    && !CompilerGenerated(t))
                .ToList();

            transpiler.Run(inputTypes);

            // var outputFilePaths = Directory.GetFiles(outputPath);
            // var expectedFileNames = outputFilePaths.Select(x => Path.GetFileName(x));

            var expectedTsFiles = Directory.GetFiles(outputPath, "*.*", SearchOption.AllDirectories)
                .Select(x => new TsFile
                {
                    Name = Path.GetFileName(x),
                    Directory = config.TargetDirectory
                        + x.Substring(
                            outputPath.Length,
                            x.Length - outputPath.Length - Path.GetFileName(x).Length),
                    Lines = File.ReadAllLines(x).ToList()
                });

            var expectedFilePaths = expectedTsFiles.Select(x => x.Directory + x.Name);
            var actualFilePaths = fileWriter.CreatedFiles.Select(x => x.Key);

            actualFilePaths.Should().BeEquivalentTo(expectedFilePaths);

            foreach (var expectedTsFile in expectedTsFiles)
            {
                var targetFileName = expectedTsFile.Directory + expectedTsFile.Name;

                var actualFileContent = fileWriter.CreatedFiles
                    .SingleOrDefault(x => x.Key == targetFileName)
                    .Value;

                actualFileContent.Should().BeEquivalentTo(expectedTsFile.Lines);
            }
        }

        bool CompilerGenerated(Type t)
        {
            var attr = Attribute.GetCustomAttribute(t, typeof(CompilerGeneratedAttribute));
            return attr != null;
        }
    }
}