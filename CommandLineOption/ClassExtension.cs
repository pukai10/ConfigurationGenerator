using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineOption
{
    public static class ClassExtension
    {
        public static List<OptionAttribute> GetOptions(this Type type)
        {

           var properties = type.GetTypeInfo().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                Console.WriteLine(properties[i].Name);
            }

            var iterfaces = ((IReflectableType)type)?.GetTypeInfo().GetInterfaces();

            if(iterfaces != null)
            {
                for (int i = 0; i < iterfaces.Length; i++)
                {
                    Console.WriteLine(iterfaces[i].Name);
                }

            }
            var options = new List<OptionAttribute>();

            return options;
        }
    }
}
