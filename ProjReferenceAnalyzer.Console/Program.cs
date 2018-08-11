using Microsoft.Extensions.DependencyInjection;
using ProjReferenceAnalyzer.Console.Startup;
using ProjReferenceAnalyzer.Core;
using ProjReferenceAnalyzer.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjReferenceAnalyzer.Console
{
    class Program
    {
        static void Main(string[] argv)
        {
            var args = new MainArgs(argv, exit: true);
            var serviceProvider = DI.ConfigureServices();

            if (args.CmdFind)
            {
                FileInfo file = null;
                bool isFile = true;

                try
                {
                    file = new FileInfo(args.ArgPath);
                }
                catch
                {
                    isFile = false;
                }

                if (isFile && file.Exists)
                {

                }
                else
                {
                    var folder = new DirectoryInfo(args.ArgPath);

                    if (folder.Exists)
                    {
                        var solutionParser = serviceProvider.GetService<ISolutionFileParser>();
                        var projectParser = serviceProvider.GetService<IProjectFileParser>();
                        List<SolutionInfo> solutions = new List<SolutionInfo>();
                        var solutionFiles = folder.GetFiles("*.sln", SearchOption.AllDirectories);

                        foreach (var solutionFile in solutionFiles)
                        {
                            solutions.Add(solutionParser.GetSolutionInfo(solutionFile));
                        }

                        RemoveDuplicateProjects(solutions);

                        var projects = solutions.SelectMany(si => si.Projects);

                        foreach (var project in projects)
                        {
                            projectParser.FindDependenciesForProject(project, projects);
                        }

                        System.Console.WriteLine($"Found {solutions.Count} solutions");
                        System.Console.WriteLine($"Found {projects.Count()} projects");
                        System.Console.WriteLine($"Found {projects.Sum(pi => pi.Dependencies.Count)} dependencies");
                        IGraphStorage graphStorage = serviceProvider.GetService<IGraphStorage>();
                        graphStorage.SerializationFormat = serviceProvider.GetService<IGraphSerializationFormat>();
                        graphStorage.Store(@"C:\Rot\TestProjectAnalyzer.gv", solutions);
                    }
                }
            }

            System.Console.ReadKey();
        }

        private static void RemoveDuplicateProjects(List<SolutionInfo> solutions)
        {
            var duplicateProjects = new Dictionary<string, List<ProjectInfo>>();
            var projects = solutions.SelectMany(si => si.Projects);

            foreach (var project in projects)
            {
                List<ProjectInfo> listOfDuplicates = null;

                if (duplicateProjects.ContainsKey(project.ProjectFile.FullName))
                {
                    listOfDuplicates = duplicateProjects[project.ProjectFile.FullName];
                }
                else
                {
                    listOfDuplicates = new List<ProjectInfo>();
                    duplicateProjects.Add(project.ProjectFile.FullName, listOfDuplicates);
                }

                listOfDuplicates.Add(project);
            }

            foreach (var projectPath in duplicateProjects.Keys)
            {
                if (duplicateProjects[projectPath].Count > 1)
                {
                    // All solutions should share the same instance of an individual project
                    var projectToKeep = duplicateProjects[projectPath].First();
                    var solutionsWithCurrentProject = solutions.FindAll(si => si.Projects.Any(pi => pi.ProjectFile.FullName == projectPath));
                    foreach (var solution in solutionsWithCurrentProject)
                    {
                        for (int index = solution.Projects.Count() - 1; index > 0; index--)
                        {
                            var projectToTest = solution.Projects.ElementAt(index);

                            if (projectToTest.ProjectFile.FullName == projectToKeep.ProjectFile.FullName && object.ReferenceEquals(projectToTest, projectToKeep))
                            {
                                solution.Projects.Remove(projectToTest);
                                solution.Projects.Add(projectToKeep);
                            }
                        }
                    }
                }
            }
        }
    }
}
