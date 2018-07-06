using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.Services
{
    public class ProjectFileParser : IProjectFileParser
    {
        public void FindDependenciesForProject(ProjectInfo project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (project.ProjectFile == null) throw new ArgumentException("ProjectFile property must be specified in order to parse the content and find dependencies.", nameof(project));
            if (!project.ProjectFile.Exists) throw new FileNotFoundException("Project file not found.", project.ProjectFile.FullName);
            string projectContent = File.ReadAllText(project.ProjectFile.FullName);
            string packagesConfigContent = GetContentOfProjectPackagesConfig(project);
            FindDependenciesForProject(project, projectContent, packagesConfigContent);
        }

        public string GetContentOfProjectPackagesConfig(ProjectInfo project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (project.ProjectFile == null) throw new ArgumentException("ProjectFile property must be specified in order to parse the content and find dependencies.", nameof(project));
            if (!project.ProjectFile.Exists) throw new FileNotFoundException("Project file not found.", project.ProjectFile.FullName);
            var packagesConfigFile = project.ProjectFile.Directory.GetFiles("packages.config").FirstOrDefault();

            if (packagesConfigFile != null)
            {
                return File.ReadAllText(packagesConfigFile.FullName);
            }

            return null;
        }

        public void FindDependenciesForProject(ProjectInfo project, string projectContent, string packagesConfigContent)
        {
            var projectXml = XDocument.Parse(projectContent);
            var packagesConfigXml = !string.IsNullOrWhiteSpace(packagesConfigContent) ? XDocument.Parse(packagesConfigContent) : null;

            FindNuGetDependenciesForProject(project, projectXml, packagesConfigXml);
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
