using ProjReferenceAnalyzer.Core;
using QuickGraph.Graphviz.Dot;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    public class DotFormat : IGraphSerializationFormat
    {
        public DotFormat()
        {
        }

        public IEnumerable<SolutionInfo> DeserializeGraph(string graphData)
        {
            // TODO: Must be able to rebuild all information for SolutionInfo, ProjectInfo etc from the Dot language.
            throw new NotImplementedException();
        }

        public string SerializeGraph(IEnumerable<SolutionInfo> solutions)
        {
            var graph = new QuickGraph.AdjacencyGraph<Node, NodeLink>();
            var projectNodes = new Dictionary<string, ProjectNode>();

            // 1. Create vertices for each unique dependency, project and solution
            foreach (var solution in solutions)
            {
                var solutionNode = new SolutionNode(solution);
                graph.AddVertex(solutionNode);

                foreach (var project in solution.Projects)
                {
                    ProjectNode projectNode = null;
                    string projectNodeKey = project.ProjectFile.FullName;

                    if (projectNodes.ContainsKey(projectNodeKey))
                    {
                        projectNode = projectNodes[projectNodeKey];
                    }
                    else
                    {
                        projectNode = new ProjectNode(project);
                        projectNodes.Add(projectNodeKey, projectNode);
                        graph.AddVertex(projectNode);
                    }

                    // 2. Create edges between solution and project vertices
                    graph.AddEdge(new NodeLink(solutionNode, projectNode));
                }
            }

            // TODO: Add enough information to the Dot data to be able to deserialize back to SolutionInfo, ProjectInfo etc.
            var g = new QuickGraph.Graphviz.GraphvizAlgorithm<Node, NodeLink>(graph);
            g.FormatEdge += (sender, e) =>
            {
                if (e.Edge.Source is SolutionNode)
                {
                    e.EdgeFormatter.Style = QuickGraph.Graphviz.Dot.GraphvizEdgeStyle.Bold;
                }
                else if (e.Edge.Source is ProjectNode && e.Edge.Target is ProjectNode)
                {
                    e.EdgeFormatter.Style = QuickGraph.Graphviz.Dot.GraphvizEdgeStyle.Solid;
                }
                else if (e.Edge.Source is ProjectNode && e.Edge.Target is NuGetPackageNode)
                {
                    e.EdgeFormatter.Style = QuickGraph.Graphviz.Dot.GraphvizEdgeStyle.Dotted;
                }
            };
            g.FormatVertex += (sender, e) =>
            {
                if (e.Vertex.Image != null)
                {
                    e.VertexFormatter.Shape = QuickGraph.Graphviz.Dot.GraphvizVertexShape.Unspecified;
                    e.VertexFormatter.Size = new GraphvizSizeF(20, 20);
                    e.VertexFormatter.Url = e.Vertex.Image;
                }
                else
                {
                    if (e.Vertex is SolutionNode)
                    {
                        e.VertexFormatter.Shape = QuickGraph.Graphviz.Dot.GraphvizVertexShape.Hexagon;
                    }
                    else if (e.Vertex is ProjectNode)
                    {
                        e.VertexFormatter.Shape = QuickGraph.Graphviz.Dot.GraphvizVertexShape.Rectangle;
                    }
                    else if (e.Vertex is NuGetPackageNode)
                    {
                        e.VertexFormatter.Shape = QuickGraph.Graphviz.Dot.GraphvizVertexShape.Record;
                    }
                }
            };
            var dot = g.Generate();
            dot = dot.Replace("URL=\"", "image=\"");

            return dot;
        }
    }
}
