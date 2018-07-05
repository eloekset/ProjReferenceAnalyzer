using ProjReferenceAnalyzer.Core;
using System.Collections.Generic;
using System.IO;

namespace ProjReferenceAnalyzer.Services
{
    public interface ISolutionFileParser
    {
        SolutionInfo GetSolutionInfo(FileInfo solutionFile);
    }
}
