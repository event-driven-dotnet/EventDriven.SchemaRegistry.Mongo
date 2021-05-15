using MongoDB.Bson.Serialization.Attributes;

namespace EventDriven.SchemaRegistry.Mongo
{
    /// <summary>
    /// Mongo schema representation.
    /// </summary>
    public class MongoSchema
    {
        /// <summary>
        /// Schema id (topic).
        /// </summary>
        [BsonId]
        public string Id { get; set; }

        /// <summary>
        /// Schema value (content).
        /// </summary>
        public string Value { get; set; }
    }
}