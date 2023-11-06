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

        public static string ConvertPath(params string[] args)
        {
            if(args != null)
            {
                return SystemPath(string.Join("/", args));
            }

            return string.Empty;
        }

        public static void SaveFile(string path,string content)
        {
            path = SystemPath(path);
            if(File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, content);
        }

        public static void SaveFile(string path, byte[] datas)
        {
            path = SystemPath(path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, datas);
        }

        public static string ReadFileText(string path,bool create = false,string defaultContent = "")
        {
            path = SystemPath(path);
            if (File.Exists(path) == false)
            {
                if(!create)
                {
                    throw new System.Exception("文件不存在：" + path);
                }

                File.WriteAllText(path, defaultContent);
            }

            return File.ReadAllText(path);
        }

        public static byte[] ReadFileBytes(string path, bool create = false, string defaultContent = "")
        {
            path = SystemPath(path);
            if (File.Exists(path) == false)
            {
                if (!create)
                {
                    throw new System.Exception("文件不存在：" + path);
                }

                File.WriteAllText(path, defaultContent);
            }

            return File.ReadAllBytes(path);
        }
    }
}
