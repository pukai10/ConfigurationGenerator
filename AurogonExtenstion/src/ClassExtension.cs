using System;
namespace AurogonExtenstion.src
{
	public static class ClassExtension
	{
		public static bool IsSame(this Type type,string typeName)
		{
			if(type == null)
			{
				throw new Exception("type is null");
			}

			if(type.Name.Equals(typeName))
			{
				return true;
			}

			if(type.Name == "System.Object")
			{
				return false;
			}

			return type.BaseType.IsSame(typeName);
		}
	}
}

