using Shouldly;
using System.IO;
using Xunit;

namespace ProjReferenceAnalyzer.Services.Tests
{
    public class SolutionFileParser_RegexLookupProjectNameAndPathShould
    {
        [Fact]
        public void FindProjectWithNamePathAndGuid()
        {
            // Arrange
            string solutionContentLine = TestData.KnownSolutionContentLineForConsoleProject;
            string solutionDirectoryPath = TestData.KnownSolutionPath;
            string expectedProjectPath = Path.Combine(solutionDirectoryPath, TestData.KnownCoreProjectPath);
            var parser = new SolutionFileParser();

            // Act
            var foundProject = parser.RegexLookupProjectNameAndPath(solutionDirectoryPath, solutionContentLine);

            // Assert
            foundProject.ShouldNotBeNull("Project was found");
            foundProject.ProjectFile.ShouldNotBeNull("Project file was found");
            foundProject.ProjectFile.FullName.ShouldBe(expectedProjectPath, "The expected project path was parsed");
        }
    }
}
