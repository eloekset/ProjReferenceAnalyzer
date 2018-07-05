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
                        List<SolutionInfo> solutions = new List<SolutionInfo>();
                        var solutionFiles = folder.GetFiles("*.sln", SearchOption.AllDirectories);

                        foreach (var solutionFile in solutionFiles)
                        {
                            solutions.Add(solutionParser.GetSolutionInfo(solutionFile));
                        }

                        System.Console.WriteLine($"Found {solutions.Count} solutions");
                        System.Console.WriteLine($"Found {solutions.Sum(si => si.Projects.Count())} projects");
                    }
                }
            }

            System.Console.ReadKey();
        }
    }
}
