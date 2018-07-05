using Shouldly;
using System.IO;
using System.Linq;
using Xunit;

namespace ProjReferenceAnalyzer.Services.Tests
{
    public class SolutionFileParser_GetProjectsShould
    {
        [Fact]
        public void ReturnListOfProjectsFromSolutionFile()
        {
            // Arrange
            string solutionContent = TestData.GetSolutionFileContent();
            string solutionDirectoryPath = TestData.KnownSolutionPath;
            var parser = new SolutionFileParser();

            // Act
            var projects = parser.GetProjects(solutionDirectoryPath, solutionContent);

            // Assert
            projects.ShouldNotBeNull("Projects were found in the solution file");
            projects.Count().ShouldBe(3, "3 known projects were found in solution");
            projects.Any(pi => pi.Name == TestData.KnownConsoleProjectName).ShouldBe(true, "Console project was found");
            projects.Any(pi => pi.Name == TestData.KnownServicesProjectName).ShouldBe(true, "Services project was found");
            projects.Any(pi => pi.Name == TestData.KnownCoreProjectName).ShouldBe(true, "Core project was found");
            projects.Any(pi => pi.ProjectFile.FullName == Path.Combine(solutionDirectoryPath, TestData.KnownConsoleProjectPath)).ShouldBe(true, "Path to Console project was as expected");
            projects.Any(pi => pi.ProjectFile.FullName == Path.Combine(solutionDirectoryPath, TestData.KnownServicesProjectPath)).ShouldBe(true, "Path to Services project was as expected");
            projects.Any(pi => pi.ProjectFile.FullName == Path.Combine(solutionDirectoryPath, TestData.KnownCoreProjectPath)).ShouldBe(true, "Path to Core project was as expected");
        }
    }
}
