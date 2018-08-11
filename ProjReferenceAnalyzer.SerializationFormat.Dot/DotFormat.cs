using ProjReferenceAnalyzer.Core;
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
            g.FormatVertex += (sender, e) =>
            {
                // This just has to exist to have labels populated
            };
            var dot = g.Generate();

            return dot;
        }
    }
}
