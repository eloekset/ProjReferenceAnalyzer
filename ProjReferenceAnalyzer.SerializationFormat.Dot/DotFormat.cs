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
            var nugetNodes = new Dictionary<string, NuGetPackageNode>();
            var gacNodes = new Dictionary<string, GacNode>();
            var fileNodes = new Dictionary<string, FileNode>();

            ProjectNode GetOrAddProjectNode(ProjectInfo project)
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

                return projectNode;
            }

            NuGetPackageNode GetOrAddNuGetNode(NuGetPackageInfo nugetPackage)
            {
                NuGetPackageNode nugetNode = null;
                string nugetNodeKey = $"{nugetPackage.PackageId}_{nugetPackage.Version}";

                if (nugetNodes.ContainsKey(nugetNodeKey))
                {
                    nugetNode = nugetNodes[nugetNodeKey];
                }
                else
                {
                    nugetNode = new NuGetPackageNode(nugetPackage);
                    nugetNodes.Add(nugetNodeKey, nugetNode);
                    graph.AddVertex(nugetNode);
                }

                return nugetNode;
            }

            GacNode GetOrAddGacNode(GacReference gacReference)
            {
                GacNode gacNode = null;
                string gacKey = gacReference.AssemblyName;

                if (gacNodes.ContainsKey(gacKey))
                {
                    gacNode = gacNodes[gacKey];
                }
                else
                {
                    gacNode = new GacNode(gacReference);
                    gacNodes.Add(gacKey, gacNode);
                    graph.AddVertex(gacNode);
                }

                return gacNode;
            }

            FileNode GetOrAddFileNode(FileReference fileReference) 
            {
                FileNode fileNode = null;
                string fileKey = fileReference.File.FullName;

                if (fileNodes.ContainsKey(fileKey))
                {
                    fileNode = fileNodes[fileKey];
                }
                else
                {
                    fileNode = new FileNode(fileReference);
                    fileNodes.Add(fileKey, fileNode);
                    graph.AddVertex(fileNode);
                }

                return fileNode;
            }

            // 1. Create vertices for each unique dependency, project and solution
            foreach (var solution in solutions)
            {
                var solutionNode = new SolutionNode(solution);
                graph.AddVertex(solutionNode);

                foreach (var project in solution.Projects)
                {
                    ProjectNode projectNode = GetOrAddProjectNode(project);

                    // 2. Create edges between solution and project vertices
                    graph.AddEdge(new NodeLink(solutionNode, projectNode));

                    // 3. Create edges for dependencies
                    foreach (var dependency in project.Dependencies)
                    {
                        if (dependency is ProjectReference)
                        {
                            ProjectInfo dependencyProject = ((ProjectReference)dependency).Project;

                            // Some project references are not resolved because project file is missing or just not checked out from source control
                            if (dependencyProject != null)
                            {
                                ProjectNode dependencyProjectNode = GetOrAddProjectNode(dependencyProject);
                                graph.AddEdge(new NodeLink(projectNode, dependencyProjectNode));
                            }
                        }
                        else if (dependency is NuGetReference)
                        {
                            NuGetPackageInfo nugetPackage = ((NuGetReference)dependency).NuGetPackage;
                            NuGetPackageNode dependencyNuGetNode = GetOrAddNuGetNode(nugetPackage);
                            graph.AddEdge(new NodeLink(projectNode, dependencyNuGetNode));
                        }
                        else if (dependency is GacReference)
                        {
                            GacNode dependencyGacNode = GetOrAddGacNode((GacReference)dependency);
                            graph.AddEdge(new NodeLink(projectNode, dependencyGacNode));
                        }
                        else if (dependency is FileReference)
                        {
                            FileNode dependencyFileNode = GetOrAddFileNode((FileReference)dependency);
                            graph.AddEdge(new NodeLink(projectNode, dependencyFileNode));
                        }
                    }
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
