using System;

namespace log4net.Extensions
{
    /// <summary>
    /// Contains <see cref="Object"/> extension methods.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> is the object is null.
        /// </summary>
        /// <param name="o">The object to check.</param>
        public static void CheckNull(this object o)
        {
            o.CheckNull(null);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> is the object is null.
        /// </summary>
        /// <param name="o">The object to check.</param>
        /// <param name="paramName">Name of the parameter to include in the exception message.</param>
        public static void CheckNull(this object o, string paramName)
        {
            if (o == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
