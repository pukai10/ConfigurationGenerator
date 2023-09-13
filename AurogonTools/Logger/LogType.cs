using System;
namespace AurogonTools
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 所有日志
        /// </summary>
        All = 0,

        /// <summary>
        /// 信息日志
        /// </summary>
        Info,

        /// <summary>
        /// 调试日志
        /// </summary>
        Debug,

        /// <summary>
        /// 警告日志
        /// </summary>
        Warning,

        /// <summary>
        /// 错误日志
        /// </summary>
        Error,

        /// <summary>
        /// 致命日志
        /// </summary>
        Fatal,

        /// <summary>
        /// 空
        /// </summary>
        None,
    }
}
