using System;
using System.Threading.Tasks;
using EventDriven.SchemaRegistry.Abstractions;
using URF.Core.Abstractions;

namespace EventDriven.SchemaRegistry.Mongo
{
    /// <inheritdoc />
    public class MongoSchemaRegistry : ISchemaRegistry
    {
        private readonly IDocumentRepository<MongoSchema> _documentRepository;

        /// <summary>
        /// MongoSchemaRegistry constructor.
        /// </summary>
        /// <param name="documentRepository">MongoDB document repository.</param>
        public MongoSchemaRegistry(IDocumentRepository<MongoSchema> documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }
        
        /// <inheritdoc />
        public async Task<Schema> GetSchema(string topic)
        {
            var schema = await _documentRepository.FindOneAsync(
                e => e.Id == topic);
            return schema != null
                ? new Schema { Topic = schema.Id, Content = schema.Value }
                : null;
        }

        /// <inheritdoc />
        public async Task<bool> AddSchema(Schema schema)
        {
            var existing = await GetSchema(schema.Topic);
            if (existing != null) return false;
            await _documentRepository.InsertOneAsync(new MongoSchema
            {
                Id = schema.Topic,
                Value = schema.Content
            });
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateSchema(Schema schema)
        {
            var existing = await GetSchema(schema.Topic);
            if (existing == null) return false;
            await _documentRepository.FindOneAndReplaceAsync(
                e => e.Id == schema.Topic, new MongoSchema
                {
                    Id = schema.Topic,
                    Value = schema.Content
                });
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveSchema(string topic)
        {
            var existing = await GetSchema(topic);
            if (existing == null) return false;
            await _documentRepository.DeleteOneAsync(e => e.Id == topic);
            return true;
        }
    }
}