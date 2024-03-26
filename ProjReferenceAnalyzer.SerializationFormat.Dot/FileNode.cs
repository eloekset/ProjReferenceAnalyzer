using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    internal class FileNode : Node
    {
        public FileNode(FileReference fileReference)
        {
            this.Name = fileReference.File.Name;
            this.Image = "./DLLFile.png";
        }
    }
}
