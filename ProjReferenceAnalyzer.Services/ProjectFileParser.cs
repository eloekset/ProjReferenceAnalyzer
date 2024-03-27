using ProjReferenceAnalyzer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ProjReferenceAnalyzer.Services
{
    public class ProjectFileParser : IProjectFileParser
    {
        public const string OldStyleProjectXmlNamespace = @"http://schemas.microsoft.com/developer/msbuild/2003";
        private const string GacReferenceIncludeRegexPattern = @"/(.+),\s?(Version=.+),\s?(Culture=.+),\s?(PublicKeyToken=\w+)";
        private readonly Regex _gacReferenceIncludeRegex;

        public ProjectFileParser() 
        {
            _gacReferenceIncludeRegex = new Regex(GacReferenceIncludeRegexPattern, RegexOptions.Compiled);
        }

        public void FindDependenciesForProject(ProjectInfo project, IEnumerable<ProjectInfo> allProjects)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (project.ProjectFile == null) throw new ArgumentException("ProjectFile property must be specified in order to parse the content and find dependencies.", nameof(project));
            if (project.ProjectFile.Exists)
            {
                project.ProjectFileExists = true;
                string projectContent = File.ReadAllText(project.ProjectFile.FullName);
                string packagesConfigContent = GetContentOfProjectPackagesConfig(project);
                FindDependenciesForProject(project, allProjects, projectContent, packagesConfigContent);
            }
            /*else
            {
                throw new FileNotFoundException("Project file not found.", project.ProjectFile.FullName);
            }*/
        }

        public string GetContentOfProjectPackagesConfig(ProjectInfo project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (project.ProjectFile == null) throw new ArgumentException("ProjectFile property must be specified in order to parse the content and find dependencies.", nameof(project));
            if (project.ProjectFile.Exists)
            {
                var packagesConfigFile = project.ProjectFile.Directory.GetFiles("packages.config").FirstOrDefault();

                if (packagesConfigFile != null)
                {
                    return File.ReadAllText(packagesConfigFile.FullName);
                }
            }
            /*else
            {
                throw new FileNotFoundException("Project file not found.", project.ProjectFile.FullName);
            }*/

            return null;
        }

        public void FindDependenciesForProject(ProjectInfo project, IEnumerable<ProjectInfo> allProjects, string projectContent, string packagesConfigContent)
        {
            var projectXml = XDocument.Parse(projectContent);
            var packagesConfigXml = !string.IsNullOrWhiteSpace(packagesConfigContent) ? XDocument.Parse(packagesConfigContent) : null;

            FindGacAndFileDependenciesForProject(project, projectXml);
            FindNuGetDependenciesForProject(project, projectXml, packagesConfigXml);
            FindProjectReferenceDependenciesForProject(project, allProjects, projectXml);
        }

        public void FindGacAndFileDependenciesForProject(ProjectInfo project, XDocument projectXml)
        {
            var references = projectXml.Descendants(XName.Get("Reference"));
            foreach (var reference in references)
            {
                var includeAttribute = reference.Attribute(XName.Get("Include"));
                var assemblyNameMatch = _gacReferenceIncludeRegex.Match(includeAttribute.Value);

                if (assemblyNameMatch.Success)
                {
                    var hintPath = reference.Descendants(XName.Get("HintPath"))
                        .Select(n => n.Value)
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(hintPath))
                    {
                        project.ProjectFile?.Directory;
                        var hintPathDirectory = new FileInfo(hintPath).Directory;
                        hintPathDirectory.
                    }

                    project.Dependencies.Add(new GacReference
                    {
                        AssemblyName = assemblyNameMatch.Groups[0].Value
                    });
                }
            }
        }

        public void FindProjectReferenceDependenciesForProject(ProjectInfo project, IEnumerable<ProjectInfo> allProjects, XDocument projectXml)
        {
            // Old style .NET Framework projects
            var projectReferencePaths = projectXml.Descendants(XName.Get("ProjectReference", OldStyleProjectXmlNamespace))
                .Attributes(XName.Get("Include"))
                .Select(a => a.Value);

            if (projectReferencePaths.Count() == 0)
            {
                // .NET Standard and .NET Core projects
                projectReferencePaths = projectXml
                    .Descendants(XName.Get("ProjectReference"))
                    .Attributes(XName.Get("Include"))
                    .Select(a => a.Value);
            }

            foreach (var projectReferencePath in projectReferencePaths)
            {
                
                project.Dependencies.Add(new ProjectReference
                {
                    Project = allProjects.FirstOrDefault(pi => new FileInfo(pi.ProjectFile.FullName).AbsolutePath().Equals(new FileInfo(Path.Combine(project.ProjectFile.Directory.FullName + Path.DirectorySeparatorChar, projectReferencePath)).AbsolutePath()))
                });
            }
        }

        public void FindNuGetDependenciesForProject(ProjectInfo project, XDocument projectXml, XDocument packagesConfigXml)
        {
            if (packagesConfigXml != null)
            {
                // Old style .NET Framework project with related packages.config for NuGet dependencies
                FindNuGetDependenciesFromPackagesConfig(project, packagesConfigXml);
            }
            else
            {
                // .NET Standard or .NET Core project
                FindNuGetDependenciesFromProjectFile(project, projectXml);
            }
        }

        public void FindNuGetDependenciesFromPackagesConfig(ProjectInfo project, XDocument packagesConfigXml)
        {
            var packageElement = packagesConfigXml.Element(XName.Get("packages", ""));

            if (packageElement != null)
            {
                var nuGetReferences = packageElement.Elements(XName.Get("package", "")).Select(p => new NuGetReference
                {
                    NuGetPackage = new NuGetPackageInfo
                    {
                        PackageId = p.Attribute(XName.Get("id", ""))?.Value,
                        Version = p.Attribute(XName.Get("version", ""))?.Value
                    }
                });

                foreach (var nuGetReference in nuGetReferences)
                {
                    project.Dependencies.Add(nuGetReference);
                }
            }
        }

        public void FindNuGetDependenciesFromProjectFile(ProjectInfo project, XDocument projectXml)
        {
            var nuGetReferences = projectXml.Descendants(XName.Get("PackageReference")).Select(pr => new NuGetReference
            {
                NuGetPackage = new NuGetPackageInfo
                {
                    PackageId = pr.Attribute(XName.Get("Include", ""))?.Value,
                    Version = pr.Attribute(XName.Get("Version", ""))?.Value
                }
            });

            foreach (var nuGetReference in nuGetReferences)
            {
                project.Dependencies.Add(nuGetReference);
            }
        }
    }
}
