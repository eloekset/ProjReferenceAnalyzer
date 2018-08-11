using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    internal class SolutionNode : Node
    {
        public SolutionNode(SolutionInfo solutionInfo)
        {
            this.Name = solutionInfo.SolutionFile.Name;
        }
    }
}
