namespace Transpiler.Tests.K
{
    public class Config : Configuration
    {
        public Config()
        {
            this.TargetDirectory = "c:\\temp\\cs2ts_test";
            this.UseNamespacesAsFolders = true;
            this.MapNamespace = ns => ns.Substring("Transpiler.Tests.K".Length);
        }
    }
}