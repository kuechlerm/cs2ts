namespace CS2TS.Tests.V
{
    public class Config : Configuration
    {
        public Config()
        {
            this.TargetDirectory = "c:\\temp\\cs2ts_test";
            this.CreateIndexFiles = true;
            this.UseNamespacesAsFolders = true;
            this.MapNamespace = ns => ns.Substring("CS2TS.Tests.V".Length);
        }

    }
}