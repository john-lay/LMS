// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbAccessExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   The db access extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.DataAccess
{
    using System.Data.Common;

    using LMS.API.Utilities;

    /// <summary>
    /// The db access extensions.
    /// </summary>
    public static class DbAccessExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The to.
        /// </summary>
        /// <param name="r">
        /// The r.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T To<T>(this DbDataReader r, string fieldName, T defaultValue = default(T))
        {
            object value = r[fieldName];
            return Converter.ConvertTo(value, defaultValue);
        }

        #endregion
    }
}