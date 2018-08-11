using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    internal class ProjectNode : Node
    {
        public ProjectNode(ProjectInfo projectInfo)
        {
            this.Name = projectInfo.Name;
        }
    }
}
