using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Amazon.DynamoDBv2.Model;
using Microsoft.VisualBasic;
using log4net.Core;
using log4net.Extensions;

namespace log4net.Appender
{
    /// <summary>
    /// Builds DynamoDb attributes for <see cref="LoggingEvent"/> items based on the persistence field type.
    /// </summary>
    public class DynamoDbAttributeBuilder
    {
        /// <summary>
        /// Builds the attribute for a binary field type.
        /// </summary>
        /// <param name="logItem">The item that will be logged.</param>
        /// <returns>A properly configured <see cref="AttributeValue"/> containing the serialized item.</returns>
        public virtual AttributeValue BuildAttributeForTypeBinary(object logItem)
        {
            logItem.CheckNull("logItem");

            using (MemoryStream serialized = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(serialized, logItem);
                return new AttributeValue { B = serialized };
            }
        }

        /// <summary>
        /// Builds the attribute for a string field type.
        /// </summary>
        /// <param name="logItem">The item that will be logged.</param>
        /// <returns>A properly configured <see cref="AttributeValue"/> containing the string representation of the item.</returns>
        public virtual AttributeValue BuildAttributeForTypeString(object logItem)
        {
            logItem.CheckNull("logItem");
            logItem.ToString().CheckNullOrEmpty("logItem");

            return new AttributeValue { S = logItem.ToString() };
        }

        /// <summary>
        /// Builds the attribute for a numeric field type.
        /// </summary>
        /// <param name="logItem">The item that will be logged.</param>
        /// <returns>A properly configured <see cref="AttributeValue"/> containing the numeric representation of the item.</returns>
        public virtual AttributeValue BuildAttributeForTypeNumeric(object logItem)
        {
            logItem.CheckNull("logItem");

            if (!Information.IsNumeric(logItem.ToString()))
            {
                throw new ArgumentException(Properties.Resources.ItemNotNumeric);
            }

            return new AttributeValue { N = logItem.ToString() };
        }
    }
}