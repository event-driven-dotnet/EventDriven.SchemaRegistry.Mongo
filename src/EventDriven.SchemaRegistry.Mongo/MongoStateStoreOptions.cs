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
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";

        /// <summary>
        /// Mongo database name.
        /// </summary>
        public string DatabaseName { get; set; } = "schema-registry";

        /// <summary>
        /// Mongo schemas collection name.
        /// </summary>
        public string SchemasCollectionName { get; set; } = "schemas";
    }
}