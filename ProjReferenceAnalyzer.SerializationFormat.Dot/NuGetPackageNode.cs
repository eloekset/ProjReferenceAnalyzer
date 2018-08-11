using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    internal class NuGetPackageNode : Node
    {
        public NuGetPackageNode(NuGetPackageInfo nuGetPackageInfo)
        {
            this.Name = nuGetPackageInfo.PackageId + "_" + nuGetPackageInfo.Version;
        }
    }
}
