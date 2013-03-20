using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Amazon.DynamoDB;
using Amazon.DynamoDB.Model;
using log4net.Appender.Properties;
using log4net.Core;

namespace log4net.Appender
{
    /// <summary>
    /// A log4net appender that writes log output to an Amazon Web Services DynamoDb database.
    /// </summary>
    public class DynamoDbAppender : BufferingAppenderSkeleton
    {
        /// <summary>
        /// Default table name. Will be used if <c>TableName</c> is not specified in configuration.
        /// </summary>
        protected const string DefaultTableName = "log4net";

        /// <summary>
        /// Gets or sets the DynamoDb table name. Defaults to "log4net".
        /// </summary>
        /// <value>The name of the table.</value>
        public virtual string TableName { get; set; }

        /// <summary>
        /// Gets or sets the table prefix. Defaults to string.empty.
        /// </summary>
        /// <value>The table prefix.</value>
        public virtual string TablePrefix { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to serialize exception objects to the database. Defaults to false.
        /// </summary>
        /// <value><c>true</c> to serialize exceptions; otherwise, <c>false</c>.</value>
        public virtual bool SerializeExceptions { get; set; }

        /// <summary>
        /// Gets the table name with the prefix, if one was set.
        /// </summary>
        /// <value>The table name with prefix.</value>
        public virtual string TableNameWithPrefix
        {
            get { return string.Concat(TablePrefix, TableName); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbAppender"/> class.
        /// </summary>
        /// <remarks>Empty default constructor</remarks>
        public DynamoDbAppender()
        {
            TableName = DefaultTableName;
            SerializeExceptions = false;
        }

        /// <summary>
        /// Sends the events.
        /// </summary>
        /// <param name="events">The events that need to be sent.</param>
        /// <remarks>
        /// The subclass must override this method to process the buffered events.
        /// </remarks>
        protected override void SendBuffer(LoggingEvent[] events)
        {
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();
                Parallel.ForEach(events, e => SendEvent(e, client));
            }
            catch (Exception ex)
            {
            
#if DEBUG
            throw;
#endif
                Trace.TraceWarning(Resources.CantLogToDynamo, ex.Message);
            }
        }

        /// <summary>
        /// Sends and individual event to AWS.
        /// </summary>
        /// <param name="logEvent">The event to log.</param>
        /// <param name="client">The current <see cref="AmazonDynamoDBClient"/> instance.</param>
        protected virtual void SendEvent(LoggingEvent logEvent, AmazonDynamoDBClient client)
        {
            if (null == logEvent)
            {
                return;
            }

            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            PutItemRequest request = new PutItemRequest
            {
                TableName = TableNameWithPrefix,
                Item = new Dictionary<string, AttributeValue>
                    {
                        { ColumnKey.Id, new AttributeValue { S = Guid.NewGuid().ToString() }},
                        { ColumnKey.Timestamp, new AttributeValue { S = logEvent.TimeStamp.ToString(CultureInfo.InvariantCulture) }}
                    }
            };

            AddToItemsIfNotEmpty(request, logEvent.RenderedMessage, ColumnKey.Message);
            AddToItemsIfNotEmpty(request, logEvent.Level.ToString(), ColumnKey.Level);
            AddToItemsIfNotEmpty(request, logEvent.UserName, ColumnKey.Username);
            AddToItemsIfNotEmpty(request, Environment.MachineName, ColumnKey.MachineName);
            AddToItemsIfNotEmpty(request, logEvent.ThreadName, ColumnKey.ThreadName);
            AddToItemsIfNotEmpty(request, logEvent.Domain, ColumnKey.AppDomain);
            AddToItemsIfNotEmpty(request, logEvent.Identity, ColumnKey.Identity);

            AddExceptionToItemsIfNotNull(logEvent, request);

            client.PutItem(request);
        }

        /// <summary>
        /// Adds the serialized <see cref="Exception"/> object to the <c>Items</c> dictionary, if one exists in the log event.
        /// </summary>
        /// <param name="logEvent">The <see cref="LoggingEvent"/> possibly containing an exception.</param>
        /// <param name="request">The <see cref="PutItemRequest"/> to be persisted.</param>
        private void AddExceptionToItemsIfNotNull(LoggingEvent logEvent, PutItemRequest request)
        {
            Debug.Assert(null != logEvent);
            Debug.Assert(null != request);

            if (null != logEvent.ExceptionObject)
            {
                AddToItemsIfNotEmpty(request, logEvent.ExceptionObject.Message, ColumnKey.ExceptionMessage);
                AddToItemsIfNotEmpty(request, logEvent.ExceptionObject.StackTrace, ColumnKey.StackTrace);

                MemoryStream exceptionSerialized = null;
                if (SerializeExceptions)
                {
                    exceptionSerialized = new MemoryStream();
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(exceptionSerialized, logEvent.ExceptionObject);
                    request.Item.Add(ColumnKey.Exception, new AttributeValue {B = exceptionSerialized});
                }

                if (null != exceptionSerialized)
                {
                    exceptionSerialized.Dispose();
                }
            }
        }

        /// <summary>
        /// Add the specifed string item to the <c>Request.Items</c> collection if it is not empty.
        /// </summary>
        /// <param name="request">The <see cref="PutItemRequest"/>.</param>
        /// <param name="logItem">The log item.</param>
        /// <param name="itemName">Name of the item (column key).</param>
        private void AddToItemsIfNotEmpty(PutItemRequest request, string logItem, string itemName)
        {
            Debug.Assert(null != request);
            Debug.Assert(!string.IsNullOrEmpty(itemName));

            if (!string.IsNullOrEmpty(logItem))
            {
                request.Item.Add(itemName, new AttributeValue { S = logItem });
            }
        }
    }
}