namespace log4net.Appender
{
    /// <summary>
    /// Contains column key names used in the DynamoDb table.
    /// </summary>
    public struct ColumnKey
    {
        /// <summary>
        /// Id column.
        /// </summary>
        public const string Id = "Id";

        /// <summary>
        /// Timestamp column.
        /// </summary>
        public const string Timestamp = "TimeStamp";

        /// <summary>
        /// Message column.
        /// </summary>
        public const string Message = "Message";

        /// <summary>
        /// Log level column.
        /// </summary>
        public const string Level = "Level";

        /// <summary>
        /// Username column.
        /// </summary>
        public const string Username = "Username";

        /// <summary>
        /// Machine name column.
        /// </summary>
        public const string MachineName = "MachineName";

        /// <summary>
        /// Thread name column.
        /// </summary>
        public const string ThreadName = "ThreadName";

        /// <summary>
        /// Identity column.
        /// </summary>
        public const string Identity = "Identity";

        /// <summary>
        /// App domain column.
        /// </summary>
        public const string AppDomain = "AppDomain";

        /// <summary>
        /// Stack trace column.
        /// </summary>
        public const string StackTrace = "StackTrace";

        /// <summary>
        /// Exception message column.
        /// </summary>
        public const string ExceptionMessage = "ExceptionMessage";

        /// <summary>
        /// Exception column.
        /// </summary>
        public const string Exception = "Exception";
    }
}
