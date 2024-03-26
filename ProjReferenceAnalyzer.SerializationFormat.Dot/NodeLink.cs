using QuickGraph;
using System;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    class NodeLink : IEdge<Node>
    {
        public Node Source { get; private set; }

        public Node Target { get; private set; }

        public string DependencyType { get; private set; }

        public NodeLink(Node source, Node target)
        {
            Source = source;
            Target = target;
            SetDependencyType();
        }

        private void SetDependencyType()
        {
            if (Source == null || Target == null) return;

            if (Source is SolutionNode)
            {
                if (Target is ProjectNode)
                {
                    DependencyType = "Project";
                }
                else
                {
                    throw new NotImplementedException($"SetDependencyType not implemented for Source type {Source.GetType().Name} and Target type {Target.GetType().Name}");
                }
            }
            else if (Source is ProjectNode)
            {
                if (Target is ProjectNode)
                {
                    DependencyType = "Project";
                }
                else if (Target is NuGetPackageNode)
                {
                    DependencyType = "NuGet";
                }
                else if (Target is GacNode)
                {
                    DependencyType = "GAC";
                }
                else if (Target is FileNode)
                {
                    DependencyType = "DLL";
                }
                else
                {
                    throw new NotImplementedException($"SetDependencyType not implemented for Source type {Source.GetType().Name} and Target type {Target.GetType().Name}");
                }
            }
            else
            {
                throw new NotImplementedException($"SetDependencyType not implemented for Source type {Source.GetType().Name} and Target type {Target.GetType().Name}");
            }
        }

        public override string ToString()
        {
            return DependencyType;
        }
    }
}
