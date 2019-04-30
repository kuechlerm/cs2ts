namespace Transpiler.Tests.F
{
    public class Config : Configuration
    {
        public Config()
        {
            this.TargetDirectory = "c:\\temp\\cs2ts_test";
            this.UseNamespacesAsFolders = true;
        }
    }
}