using Microsoft.Extensions.DependencyInjection;
using ProjReferenceAnalyzer.Core;
using ProjReferenceAnalyzer.SerializationFormat;
using ProjReferenceAnalyzer.SerializationFormat.Dot;
using ProjReferenceAnalyzer.Services;
using ProjReferenceAnalyzer.Storage;
using System;

namespace ProjReferenceAnalyzer.Console.Startup
{
    internal class DI
    {
        public static IServiceProvider ConfigureServices(MainArgs args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISolutionFileParser, SolutionFileParser>();
            services.AddSingleton<IProjectFileParser, ProjectFileParser>();

            if (args.OptJson)
            {
                services.AddSingleton<IGraphSerializationFormat, JsonFormat>();
            }
            else
            {
                services.AddSingleton<IGraphSerializationFormat, DotFormat>();
            }
            
            services.AddSingleton<IGraphStorage, LocalFileSystemStorage>();

            return services.BuildServiceProvider();
        }
    }
}
