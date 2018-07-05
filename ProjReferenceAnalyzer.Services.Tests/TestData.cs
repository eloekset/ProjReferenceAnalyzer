using System.IO;

namespace ProjReferenceAnalyzer.Services.Tests
{
    internal class TestData
    {
        public const string VisualStudioSolutionProjectGuid = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
        public const string KnownSolutionPath = @"C:\GitHub\ProjReferenceAnalyzer\";
        public const string KnownConsoleProjectName = @"ProjReferenceAnalyzer.Console";
        public const string KnownConsoleProjectPath = @"ProjReferenceAnalyzer.Console\ProjReferenceAnalyzer.Console.csproj";
        public const string KnownConsoleProjectGuid = "25910DBF-4A93-4283-ADA6-83A17A288FF3";
        public const string KnownSolutionContentLineForConsoleProject = @"Project(""{" + VisualStudioSolutionProjectGuid + @"}"") = ""{" + KnownCoreProjectName + @""", """ + KnownCoreProjectPath + @""", @""{" + KnownConsoleProjectGuid + @"}""";
        public const string KnownServicesProjectName = @"ProjReferenceAnalyzer.Services";
        public const string KnownServicesProjectPath = @"ProjReferenceAnalyzer.Services\ProjReferenceAnalyzer.Services.csproj";
        public const string KnownServicesProjectGuid = "235427A3-68CF-43B3-852E-067092D45896";
        public const string KnownCoreProjectName = @"ProjReferenceAnalyzer.Core";
        public const string KnownCoreProjectPath = @"ProjReferenceAnalyzer.Core\ProjReferenceAnalyzer.Core.csproj";
        public const string KnownCoreProjectGuid = "3B6CD151-3DEE-4913-9CEC-8C36C8D3A763";
        
        public static string GetSolutionFileContent()
        {
            return ReadStringContentOfResourceFile(Properties.Resources.ProjReferenceAnalyzer);
        }

        private static string ReadStringContentOfResourceFile(byte[] data)
        {
            using (Stream stream = new MemoryStream(data))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}
