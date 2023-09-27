using System.Collections.Generic;
using System.IO;

namespace AurogonTools
{
    public static class IOHelper
    {
        public static FileInfo[] GetAllFileInfos(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.GetFiles(searchPattern, searchOption);
        }

    }
}
