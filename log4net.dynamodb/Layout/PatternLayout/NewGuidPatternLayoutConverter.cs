using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;

namespace log4net.Layout.PatternLayout
{
    /// <summary>
    /// Pattern layout converter that generates a new Guid.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class NewGuidPatternLayoutConverter : PatternLayoutConverter
    {
        /// <summary>
        /// Converts the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="loggingEvent">The logging event.</param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            writer.Write(Guid.NewGuid().ToString());
        }
    }
}