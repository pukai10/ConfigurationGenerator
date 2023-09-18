using System;
namespace AurogonCodeGenerator
{
	/// <summary>
	/// 代码成员
	/// </summary>
	public abstract class CodeMemberInfo
	{
		private string m_name;
		public string Name => m_name;

		public CodeMemberInfo(string name)
		{
			m_name = name;
		}
	}
}

