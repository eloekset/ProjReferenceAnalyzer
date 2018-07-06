using System.IO;

namespace ProjReferenceAnalyzer.Services
{
    public static class FileInfoExtensions
    {
        public static string AbsolutePath(this FileInfo fileInfo)
        {
            string absolutePath = fileInfo.FullName;

            while(absolutePath.Contains(".." + Path.DirectorySeparatorChar))
            {
                int indexOfUpOneLevel = absolutePath.IndexOf(".." + Path.DirectorySeparatorChar);
                int indexOfPreviousDirectory = absolutePath.IndexOf(Path.DirectorySeparatorChar, indexOfUpOneLevel - 2);
                absolutePath = absolutePath.Substring(0, indexOfPreviousDirectory) + absolutePath.Substring(indexOfUpOneLevel + 2);
            }

            return absolutePath;
        }
    }
}
