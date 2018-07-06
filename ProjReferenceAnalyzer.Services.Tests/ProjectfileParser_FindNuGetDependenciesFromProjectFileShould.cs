using ProjReferenceAnalyzer.Core;
using Shouldly;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace ProjReferenceAnalyzer.Services.Tests
{
    public class ProjectfileParser_FindNuGetDependenciesFromProjectFileShould
    {
        [Fact]
        public void FindPackageRefElementsInNetStandardProject()
        {
            // Arrange
            string netStandardProjectFileContent = TestData.GetNetStandardProjectFileContent();
            var projectContent = XDocument.Parse(netStandardProjectFileContent);
            var project = new ProjectInfo
            {
                ProjectFile = new FileInfo(Path.Combine(TestData.KnownSolutionPath, TestData.KnownTestsProjectPath))
            };
            var parser = new ProjectFileParser();

            // Act
            parser.FindNuGetDependenciesFromProjectFile(project, projectContent);

            // Assert
            project.Dependencies.ShouldNotBeNull("Dependencies property is intialized.");
            project.Dependencies.Count.ShouldBeGreaterThanOrEqualTo(4, "At least 4 known NuGet referencies were found");
            project.Dependencies.Any(d => d.Name == "Shouldly").ShouldBeTrue("The Shouldly NuGet reference was found");
            project.Dependencies.Any(d => d.Name == "xunit").ShouldBeTrue("The xunit NuGet reference was found");
        }
    }
}
