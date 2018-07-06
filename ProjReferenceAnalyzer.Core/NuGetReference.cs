namespace ProjReferenceAnalyzer.Core
{
    public class NuGetReference : Dependency
    {
        public NuGetPackageInfo NuGetPackage { get; set; }
        public override string Name => NuGetPackage != null ? NuGetPackage.PackageId : string.Empty;
    }
}
