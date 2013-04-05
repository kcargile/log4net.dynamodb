using System;

namespace log4net.Extensions
{
    /// <summary>
    /// Contains <see cref="Object"/> extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the string is null or empty when trimmed.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>
        /// 	<c>true</c> if the string is null or empty when trimmed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmptyTrimmed(this string s)
        {
            return String.IsNullOrEmpty((s != null) ? s.Trim() : null);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null or empty.
        /// </summary>
        /// <param name="s">The string.</param>
        public static void CheckNullOrEmpty(this string s)
        {
            s.CheckNullOrEmpty(null);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null or empty.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="obj">Reference to the original string.</param>
        public static void CheckNullOrEmpty(this string s, string obj)
        {
            if (s.IsNullOrEmptyTrimmed())
            {
                throw new ArgumentException(Properties.Resources.StringCannotBeEmpty, obj);
            }
        }
    }
}
