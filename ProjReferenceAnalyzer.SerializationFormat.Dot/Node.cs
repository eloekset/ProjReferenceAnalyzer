namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    internal abstract class Node
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}