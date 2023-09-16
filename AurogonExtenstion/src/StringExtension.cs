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

    /// <summary>
    /// 字符串转整数
    /// </summary>
    /// <param name="str">需要转换的字符串</param>
    /// <param name="defaultValue">失败的时候的默认值</param>
    /// <param name="forceParse">true:忽略转失败，失败时输出默认指，false:转失败抛出异常</param>
    /// <returns></returns>
    public static int ToInt(this string str,int defaultValue = 0,bool forceParse = true)
    {
        int result = 0;
        if(int.TryParse(str,out result) == false && !forceParse)
        {
            throw new Exception($"字符串转整型失败，str:{str}");
        }

        return result;
    }

    /// <summary>
    /// 字符串转浮点数
    /// </summary>
    /// <param name="str">需要转换的字符串</param>
    /// <param name="defaultValue">失败的时候的默认值</param>
    /// <param name="forceParse">true:忽略转失败，失败时输出默认指，false:转失败抛出异常</param>
    /// <returns></returns>
    public static float ToFloat(this string str,float defaultValue = 0f,bool forceParse = true)
    {
        float result = defaultValue;
        if (float.TryParse(str, out result) == false && !forceParse)
        {
            throw new Exception($"字符串转浮点数失败，str:{str}");
        }

        return result;
    }

    /// <summary>
    /// 字符串转boolean
    /// </summary>
    /// <param name="str">需要转换的字符串</param>
    /// <param name="defaultValue">失败的时候的默认值</param>
    /// <param name="forceParse">true:忽略转失败，失败时输出默认指，false:转失败抛出异常</param>
    /// <returns></returns>
    public static bool ToBoolean(this string str,bool defaultValue = false,bool forceParse = true)
    {
        bool result = defaultValue;
        if(bool.TryParse(str,out result) == false && !forceParse)
        {
            throw new Exception($"字符串转布尔值失败，str:{str}");
        }

        return result;
    }
}
