namespace LMS.API.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Web;
    using LMS.API.Utilities;

    public static class DbAccessExtensions
    {
        public static T To<T>(this DbDataReader r, string fieldName, T defaultValue = default(T))
        {
            object value = r[fieldName];
            return Converter.ConvertTo<T>(value, defaultValue);
        }
    }
}