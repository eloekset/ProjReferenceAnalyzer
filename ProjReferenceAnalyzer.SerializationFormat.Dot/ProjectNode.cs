using ProjReferenceAnalyzer.Core;

namespace ProjReferenceAnalyzer.SerializationFormat.Dot
{
    internal class ProjectNode : Node
    {
        public ProjectNode(ProjectInfo projectInfo)
        {
            this.Name = projectInfo.Name;
            switch(projectInfo.ProjectFile?.Extension)
            {
                case ".csproj":
                    this.Image = "./CSProj.png";
                    break;
                case ".btproj":
                    this.Image = "./BTProj.png";
                    break;
                case ".sln":
                    this.Image = "./SLNFile.png";
                    break;
                default:
                    this.Image = "./MissingFile.svg";
                    break;
            }

            
        }
    }
}
