using ProjReferenceAnalyzer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ProjReferenceAnalyzer.Services
{
    public class SolutionFileParser : ISolutionFileParser
    {
        public const string SolutionProjectNameAndPathRegexPattern =
@"(?x)
^ Project \("" { FAE04EC0-301F-11D3-BF4B-00C04F79EFBC } ""\)
\s* = \s*
""(?<name> [^""""]*)""\,\s?
""(?<path> [^""""]*)""";
        public Regex SolutionProjectNameAndPathRegex { get; private set; }

        public SolutionFileParser()
        {
            SolutionProjectNameAndPathRegex = new Regex(SolutionProjectNameAndPathRegexPattern);
        }

        public SolutionInfo GetSolutionInfo(FileInfo solutionFile)
        {
            return new SolutionInfo
            {
                SolutionFile = solutionFile,
                Projects = GetProjects(solutionFile)
            };
        }

        public IEnumerable<ProjectInfo> GetProjects(FileInfo solutionFile)
        {
            if (solutionFile == null) throw new ArgumentNullException(nameof(solutionFile));
            if (!solutionFile.Exists) throw new FileNotFoundException("Solution file not found.", solutionFile.FullName);
            string solutionContent = File.ReadAllText(solutionFile.FullName);

            return GetProjects(solutionFile.Directory.FullName, solutionContent);
        }

        public IEnumerable<ProjectInfo> GetProjects(string solutionDirectoryPath, string solutionContent)
        {
            var projectDependencies = new List<ProjectInfo>();
            
            using (StringReader sr = new StringReader(solutionContent))
            {
                string solutionContentLine = sr.ReadLine();
                while (solutionContentLine != null)
                {
                    var projectFound = RegexLookupProjectNameAndPath(solutionDirectoryPath, solutionContentLine);
                    if (projectFound != null) projectDependencies.Add(projectFound);
                    solutionContentLine = sr.ReadLine();
                }
            }

            return projectDependencies;
        }

        public ProjectInfo RegexLookupProjectNameAndPath(string solutionDirectoryPath, string solutionContentLine)
        {
            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("x", @"http://schemas.microsoft.com/developer/msbuild/2003");
            var matches = SolutionProjectNameAndPathRegex.Matches(solutionContentLine);

            foreach (Match match in matches)
            {
                var projectName = match.Groups["name"].Value;
                var projectPath = match.Groups["path"].Value;
                var projectFileInfo = new FileInfo(Path.Combine(solutionDirectoryPath, projectPath));

                return new ProjectInfo
                {
                    ProjectFile = projectFileInfo
                };
                if (projectFileInfo.Exists)
                {
                    var projectContent = File.ReadAllText(projectFileInfo.FullName);
                    var proj = XDocument.Parse(projectContent);
                    var projRefElements = proj.XPathSelectElements("/x:Project/x:ItemGroup/x:Reference", nsManager);

                    // TODO: https://gist.github.com/jstangroome/557222
                }

            }

            return null;
        }
    }
}
