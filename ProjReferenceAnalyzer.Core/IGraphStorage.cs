using System.Collections.Generic;

namespace ProjReferenceAnalyzer.Core
{
    public interface IGraphStorage
    {
        IGraphSerializationFormat SerializationFormat { get; set; }
        void Store(string graphIdentifier, IEnumerable<SolutionInfo> solutions);
        IEnumerable<SolutionInfo> Load(string graphIdentifier);
    }
}
