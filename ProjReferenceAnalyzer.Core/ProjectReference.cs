using System.IO;

namespace ProjReferenceAnalyzer.Core
{
    public class ProjectReference : Dependency
    {
        public ProjectInfo Project { get; set; }
        public override string Name => Project != null ? Project.Name : string.Empty;
    }
}
