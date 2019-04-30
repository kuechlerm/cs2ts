namespace Transpiler.Tests.D
{
    public class Config : Configuration
    {
        public Config()
        {
            this.TargetDirectory = "c:\\temp\\cs2ts_test";
            this.PrintGeneratedFileText = true;
        }
    }
}