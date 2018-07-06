using ProjReferenceAnalyzer.Core;
using System.Collections.Generic;

namespace ProjReferenceAnalyzer.Services
{
    public interface IProjectFileParser
    {
        void FindDependenciesForProject(ProjectInfo project, IEnumerable<ProjectInfo> allProjects);
    }
}
