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

                        var projects = solutions.SelectMany(si => si.Projects);

                        foreach (var project in projects)
                        {
                            projectParser.FindDependenciesForProject(project);
                        }

                        System.Console.WriteLine($"Found {solutions.Count} solutions");
                        System.Console.WriteLine($"Found {projects.Count()} projects");
                        System.Console.WriteLine($"Found {projects.Sum(pi => pi.Dependencies.Count)} dependencies");
                    }
                }
            }

            System.Console.ReadKey();
        }
    }
}
