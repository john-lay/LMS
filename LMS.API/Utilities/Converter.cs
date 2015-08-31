// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Converter.cs" company="">
//   
// </copyright>
// <summary>
//   The converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Utilities
{
    using System;

    /// <summary>
    /// The converter.
    /// </summary>
    public static class Converter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert to.
        /// </summary>
        /// <param name="valueToBeConverted">
        /// The value to be converted.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T ConvertTo<T>(object valueToBeConverted, T defaultValue)
        {
            if (!(valueToBeConverted is DBNull) && valueToBeConverted is IConvertible)
            {
                // it needs to work with nullable types
                Type type = typeof(T);
                Type underlyingType = Nullable.GetUnderlyingType(type);

                return (T)Convert.ChangeType(valueToBeConverted, underlyingType != null ? underlyingType : type);
            }

            if (valueToBeConverted is T)
            {
                return (T)valueToBeConverted;
            }

            return defaultValue;
        }

        /// <summary>
        /// The convert to.
        /// </summary>
        /// <param name="valueToBeConverted">
        /// The value to be converted.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T ConvertTo<T>(object valueToBeConverted)
        {
            return ConvertTo(valueToBeConverted, default(T));
        }

        #endregion
    }
}