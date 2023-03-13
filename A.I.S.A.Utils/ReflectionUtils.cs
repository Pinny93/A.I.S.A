using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.I.S.A.Utils
{
    public static class ReflectionUtils
    {
        public static bool IsNullableType(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && 
                   typeToConvert
                       .GetGenericTypeDefinition()
                       .IsSubclassOf(typeof(Nullable<>));
        }
    }
}
