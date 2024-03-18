using System;
using System.Collections.Generic;
using System.IO;

namespace AurogonTools
{
    /// <summary>
    /// IO帮助类
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// 获取路径下所有文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static FileInfo[] GetAllFileInfos(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.GetFiles(searchPattern, searchOption);
        }

        /// <summary>
        /// 对应系统路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SystemPath(this string path)
        {
#if SYSTEM_MACOS
            path = path.Replace("\\", "/");
#elif SYSTEM_WINDOWS
            path = path.Replace("/", "\\");
#endif
            return path;
        }

#if SYSTEM_MACOS
        /// <summary>
        /// 系统路径分隔符
        /// </summary>
        public static readonly string SystemPathSeparator = "/";

#elif SYSTEM_WINDOWS

        /// <summary>
        /// 系统路径分隔符
        /// </summary>
        public static readonly string SystemPathSeparator = "\\";
        
#endif

        /// <summary>
        /// 连接路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string ConcatPath(this string path,params string[] args)
        {
            if(args != null)
            {
                return SystemPath($"{path}{SystemPathSeparator}{string.Join("/", args)}");
            }

            return string.Empty;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void SaveFile(string path,string content)
        {
            path = SystemPath(path);
            if(File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, content);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="datas"></param>
        public static void SaveFile(string path, byte[] datas)
        {
            path = SystemPath(path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, datas);
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <param name="defaultContent"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
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

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <param name="defaultContent"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
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
