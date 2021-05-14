namespace EventDriven.SchemaRegistry.Mongo
{
    /// <summary>
    /// Mongo state Store options.
    /// </summary>
    public class MongoStateStoreOptions
    {
        /// <summary>
        /// Mongo connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Mongo database name.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Mongo schemas collection name.
        /// </summary>
        public string SchemasCollectionName { get; set; }
    }
}