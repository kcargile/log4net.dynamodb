using System;
using System.Diagnostics;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using log4net.Extensions;

namespace log4net.Appender
{
    /// <summary>
    /// Data writer for persisting data to Amazon DynamoDb tables.
    /// </summary>
    public class DynamoDbDataWriter : IDataWriter<PutItemRequest>
    {
        /// <summary>
        /// Default service endpoint. Will be used if none is specified.
        /// </summary>
        protected const string DefaultServiceEndpoint = "http://dynamodb.us-east-1.amazonaws.com";

        /// <summary>
        /// Amazon DynamoDb client.
        /// </summary>
        protected AmazonDynamoDBClient DynamoDbClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbDataWriter"/> class.
        /// </summary>
        public DynamoDbDataWriter() : this(DefaultServiceEndpoint)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbDataWriter"/> class.
        /// </summary>
        /// <param name="endpoint">The AWS DynamoDb service endpoint.</param>
        public DynamoDbDataWriter(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                endpoint = DefaultServiceEndpoint;
            }

            DynamoDbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = endpoint });
        }

        /// <summary>
        /// Writes the specified item to SimpleDb.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Write(PutItemRequest item)
        {
            item.CheckNull("item");

            try
            {
                DynamoDbClient.PutItem(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error writing to DynamoDb: {0}.", ex);
#if DEBUG
                throw;
#endif
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != DynamoDbClient)
                {
                    DynamoDbClient.Dispose();
                    DynamoDbClient = null;
                }
            }
        }
    }
}
