using System.Collections.Generic;

namespace ProjReferenceAnalyzer.Core
{
    public abstract class Dependency
    {
        public abstract string Name { get; }
        public ICollection<Dependency> Dependencies { get; } = new List<Dependency>();
    }
}
