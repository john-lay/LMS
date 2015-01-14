using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.API.Utilities
{
    public static class Converter
    {
        public static T ConvertTo<T>(object valueToBeConverted, T defaultValue)
        {
            if (!(valueToBeConverted is DBNull) && valueToBeConverted is IConvertible)
            {
                //it needs to work with nullable types
                Type type = typeof(T);
                Type underlyingType = Nullable.GetUnderlyingType(type);

                return (T)Convert.ChangeType(valueToBeConverted, underlyingType != null ? underlyingType : type);
            }

            if (valueToBeConverted is T)
                return (T)valueToBeConverted;

            return defaultValue;
        }

        public static T ConvertTo<T>(object valueToBeConverted)
        {
            return ConvertTo<T>(valueToBeConverted, default(T));
        }
    }
}
