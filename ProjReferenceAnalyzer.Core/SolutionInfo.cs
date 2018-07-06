using System.Collections.Generic;
using System.IO;

namespace ProjReferenceAnalyzer.Core
{
    public class SolutionInfo
    {
        public FileInfo SolutionFile { get; set; }
        public IList<ProjectInfo> Projects { get; set; } = new List<ProjectInfo>();
    }
}
