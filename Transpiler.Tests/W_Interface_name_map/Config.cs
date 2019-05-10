namespace CS2TS.Tests.W
{
    public class Config : Configuration
    {
        public Config()
        {
            this.TargetDirectory = "c:\\temp\\cs2ts_test";
            this.MapName = name => "I" + name;
        }
    }
}