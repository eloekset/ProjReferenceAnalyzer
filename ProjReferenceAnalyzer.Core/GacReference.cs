namespace ProjReferenceAnalyzer.Core
{
    public class GacReference : Dependency
    {
        public string AssemblyName { get; set; }
        public override string Name => AssemblyName;
    }
}
