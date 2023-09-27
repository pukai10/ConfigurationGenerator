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


        public static string SystemPath(this string path)
        {
#if SYSTEM_MACOS
            path = path.Replace("\\", "/");
#elif SYSTEM_WINDOWS
            path = path.Replace("/", "\\");
#endif
            return path;
        }
    }
}
