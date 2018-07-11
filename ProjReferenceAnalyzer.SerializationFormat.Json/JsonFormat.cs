using Newtonsoft.Json;
using ProjReferenceAnalyzer.Core;
using System.Collections.Generic;

namespace ProjReferenceAnalyzer.SerializationFormat
{
    public class JsonFormat : IGraphSerializationFormat
    {
        public IEnumerable<SolutionInfo> DeserializeGraph(string graphData)
        {
            return JsonConvert.DeserializeObject<IEnumerable<SolutionInfo>>(graphData);
        }

        public string SerializeGraph(IEnumerable<SolutionInfo> solutions)
        {
            return JsonConvert.SerializeObject(solutions);
        }
    }
}
