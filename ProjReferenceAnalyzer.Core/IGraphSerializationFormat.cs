using System.Collections.Generic;

namespace ProjReferenceAnalyzer.Core
{
    public interface IGraphSerializationFormat
    {
        string SerializeGraph(IEnumerable<SolutionInfo> solutions);
        IEnumerable<SolutionInfo> DeserializeGraph(string graphData);
    }
}
