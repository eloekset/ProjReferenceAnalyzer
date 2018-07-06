using ProjReferenceAnalyzer.Core;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace ProjReferenceAnalyzer.Services.Tests
{
    public class ProjectFileParser_FindNuGetDependenciesFromPackagesConfigShould
    {
        [Fact]
        public void FindPackageElementsInNPackagesConfigFile()
        {
            // Arrange
            string packagesConfigContent = TestData.GetPackagesConfigFileContent();
            var packagesConfig = XDocument.Parse(packagesConfigContent);
            var project = new ProjectInfo
            {
                ProjectGuid = new Guid(TestData.KnownConsoleProjectGuid),
                ProjectFile = new FileInfo(Path.Combine(TestData.KnownSolutionPath, TestData.KnownConsoleProjectPath))
            };
            var parser = new ProjectFileParser();

            // Act
            parser.FindNuGetDependenciesFromPackagesConfig(project, packagesConfig);

            // Assert
            project.Dependencies.ShouldNotBeNull("Dependencies property is intialized.");
            project.Dependencies.Count.ShouldBeGreaterThanOrEqualTo(3, "At least 3 known NuGet referencies were found");
            project.Dependencies.Any(d => d.Name == "docopt.net").ShouldBeTrue("The docopt.net NuGet reference was found");
            project.Dependencies.Any(d => d.Name == "Microsoft.Extensions.DependencyInjection").ShouldBeTrue("The Microsoft.Extensions.DependencyInjection NuGet reference was found");
        }
    }
}
