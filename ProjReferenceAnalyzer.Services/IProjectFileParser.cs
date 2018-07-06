using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.Services
{
    public interface IProjectFileParser
    {
        void FindDependenciesForProject(ProjectInfo project);
    }
}
