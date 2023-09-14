using System;

/// <summary>
/// String 扩展
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 单char转string
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string OneCharToString(this char c)
    {
        return new string(c, 1);
    }
}
