using EventDriven.DependencyInjection.URF.Mongo;

namespace EventDriven.SchemaRegistry.Mongo
{
    /// <summary>
    /// Mongo state Store options.
    /// </summary>
    public class MongoStateStoreOptions : IMongoDbSettings
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
        public string CollectionName { get; set; } = "schemas";
    }
}