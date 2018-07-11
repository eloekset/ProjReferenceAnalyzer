using Microsoft.Extensions.DependencyInjection;
using ProjReferenceAnalyzer.Core;
using ProjReferenceAnalyzer.SerializationFormat;
using ProjReferenceAnalyzer.Services;
using ProjReferenceAnalyzer.Storage;
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
            services.AddSingleton<IGraphSerializationFormat, JsonFormat>();
            services.AddSingleton<IGraphStorage, LocalFileSystemStorage>();

            return services.BuildServiceProvider();
        }
    }
}
