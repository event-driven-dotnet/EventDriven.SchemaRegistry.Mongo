using MongoDB.Driver;

namespace EventDriven.SchemaRegistry.Mongo
{
    /// <summary>
    /// Mongo Schema Registry DbContext
    /// </summary>
    public class MongoSchemaRegistryDbContext
    {
        /// <summary>
        /// MongoSchemaRegistryDbContext constructor.
        /// </summary>
        /// <param name="client">Mongo client.</param>
        /// <param name="schemasDatabaseName">Schemas database name.</param>
        /// <param name="schemasCollectionName">Schemas collection name.</param>
        public MongoSchemaRegistryDbContext(IMongoClient client,
            string schemasDatabaseName, string schemasCollectionName)
        {
            var database = client.GetDatabase(schemasDatabaseName);
            MongoSchemas = database.GetCollection<MongoSchema>(schemasCollectionName);
        }

        /// <summary>
        /// Mongo schemas collection.
        /// </summary>
        public IMongoCollection<MongoSchema> MongoSchemas { get; }
    }
}