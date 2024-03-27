using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    internal class GacNode : Node
    {
        public GacNode(GacReference gacReference)
        {
            this.Name = gacReference.AssemblyName;
            this.Image = "./GACFile.png";
        }
    }
}
