using Amazon.DynamoDBv2.Model;
using log4net.Core;
using log4net.Extensions;
using log4net.Layout;

namespace log4net.Appender
{
    /// <summary>
    /// Parameter type used by the <see cref="DynamoDbAppender"/>.
    /// </summary>
    public class DynamoDbAppenderParameter
    {
        /// <summary>
        /// DyanmoDb parameter types.
        /// </summary>
        public enum ParameterType
        {
            /// <summary>
            /// String type. This is the default parameter type.
            /// </summary>
            S = 0,

            /// <summary>
            /// Numeric type.
            /// </summary>
            N = 2,

            /// <summary>
            /// Binary type.
            /// </summary>
            B = 4
        }

        /// <summary>
        /// Gets or sets the name of this parameter.
        /// </summary>
        /// <value>
        /// The name of this parameter.
        /// </value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter type. Synonymous wth DynamoDb field type identifiers.
        /// </summary>
        /// <value>
        /// The type of this parameter.
        /// </value>
        public virtual ParameterType Type { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IRawLayout"/> to use to render the logging event into an object for this parameter.
        /// </summary>
        /// <value>
        /// The <see cref="IRawLayout"/> used to render the logging event into an object for this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="IRawLayout"/> that renders the value for this parameter.
        /// </para>
        /// <para>
        /// The <see cref="RawLayoutConverter"/> can be used to adapt any <see cref="ILayout"/> into a <see cref="IRawLayout"/> for use in the property.
        /// </para>
        /// </remarks>
        public IRawLayout Layout { get; set; }

        /// <summary>
        /// Formats and adds the parameter to the <see cref="PutItemRequest"/>.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="loggingEvent">The logging event.</param>
        /// <returns>Chainable reference to the original request.</returns>
        public virtual PutItemRequest AddFormatParameter(PutItemRequest request, LoggingEvent loggingEvent)
        {
            request.CheckNull("request");
            loggingEvent.CheckNull("loggingEvent");

            object formattedValue = Layout.Format(loggingEvent);
            return AddToItemsIfNotNullOrEmpty(request, formattedValue);
        }

        /// <summary>
        /// Add the specifed string item to the <c>Request.Items</c> collection if it is not empty.
        /// </summary>
        /// <param name="request">The <see cref="PutItemRequest"/>.</param>
        /// <param name="logItem">The log item.</param>
        /// <returns>Chainable reference to the original request.</returns>
        protected virtual PutItemRequest AddToItemsIfNotNullOrEmpty(PutItemRequest request, object logItem)
        {
            request.CheckNull("request");

            if (null != logItem && !string.IsNullOrEmpty(logItem.ToString()))
            {
                DynamoDbAttributeBuilder builder = new DynamoDbAttributeBuilder();
                switch (Type)
                {
                    case ParameterType.B:
                        request.Item.Add(Name, builder.BuildAttributeForTypeBinary(logItem));
                        break;
                    case ParameterType.N:
                        request.Item.Add(Name, builder.BuildAttributeForTypeNumeric(logItem));
                        break;
                    default:
                        request.Item.Add(Name, builder.BuildAttributeForTypeString(logItem));
                        break;
                }
            }

            return request;
        }
    }
}
