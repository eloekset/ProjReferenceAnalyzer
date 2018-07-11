using ProjReferenceAnalyzer.Core;
using System.Collections.Generic;
using System.IO;

namespace ProjReferenceAnalyzer.Storage
{
    public class LocalFileSystemStorage : IGraphStorage
    {
        public IGraphSerializationFormat SerializationFormat { get; set; }

        public IEnumerable<SolutionInfo> Load(string graphIdentifier)
        {
            string graphData = File.ReadAllText(graphIdentifier);
            return SerializationFormat.DeserializeGraph(graphData);
        }

        public void Store(string graphIdentifier, IEnumerable<SolutionInfo> solutions)
        {
            string graphData = SerializationFormat.SerializeGraph(solutions);
            File.WriteAllText(graphIdentifier, graphData);
        }
    }
}
