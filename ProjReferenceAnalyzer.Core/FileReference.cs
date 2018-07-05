using System.IO;

namespace ProjReferenceAnalyzer.Core
{
    public class FileReference : Dependency
    {
        public FileInfo File { get; set; }
        public override string Name => File != null ? File.Name.Substring(0, (File.Name.Length - File.Extension.Length)) : string.Empty;
    }
}
