namespace log4net.Appender
{
    /// <summary>
    /// A writer for generic data.
    /// </summary>
    public interface IDataWriter<in T>
    {
        /// <summary>
        /// Writes the specified object to persistent storage.
        /// </summary>
        /// <param name="item">The item.</param>
        void Write(T item);
    }
}
