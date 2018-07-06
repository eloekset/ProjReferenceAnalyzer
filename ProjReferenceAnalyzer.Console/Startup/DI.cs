using Microsoft.Extensions.DependencyInjection;
using ProjReferenceAnalyzer.Services;
using System;

namespace ProjReferenceAnalyzer.Console.Startup
{
    internal class DI
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISolutionFileParser, SolutionFileParser>();
            services.AddSingleton<IProjectFileParser, ProjectFileParser>();

            return services.BuildServiceProvider();
        }
    }
}
