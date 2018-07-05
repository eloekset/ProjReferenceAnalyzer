using System;
using System.Collections.Generic;
using System.IO;

namespace ProjReferenceAnalyzer.Core
{
    public class ProjectInfo
    {
        public Guid ProjectGuid { get; set; }
        public FileInfo ProjectFile { get; set; }
        public string Name => ProjectFile != null ? ProjectFile.Name.Substring(0, (ProjectFile.Name.Length - ProjectFile.Extension.Length)) : string.Empty;
        public ICollection<Dependency> Dependencies { get; } = new List<Dependency>();
    }
}
