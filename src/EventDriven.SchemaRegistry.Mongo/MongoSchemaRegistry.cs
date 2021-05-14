using System;
using System.Threading.Tasks;
using EventDriven.SchemaRegistry.Abstractions;
using URF.Core.Abstractions;

namespace EventDriven.SchemaRegistry.Mongo
{
    /// <inheritdoc />
    public class MongoSchemaRegistry : ISchemaRegistry
    {
        private readonly IDocumentRepository<Schema> _documentRepository;

        /// <summary>
        /// MongoSchemaRegistry constructor.
        /// </summary>
        /// <param name="documentRepository">MongoDB document repository.</param>
        public MongoSchemaRegistry(IDocumentRepository<Schema> documentRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }
        
        /// <inheritdoc />
        public async Task<Schema> GetSchema(string topic) =>
            await _documentRepository.FindOneAsync(
                e => string.CompareOrdinal(e.Topic, topic) == 0) ;

        /// <inheritdoc />
        public async Task<bool> AddSchema(Schema schema)
        {
            var existing = await GetSchema(schema.Topic);
            if (existing != null) return false;
            await _documentRepository.InsertOneAsync(schema);
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateSchema(Schema schema)
        {
            var existing = await GetSchema(schema.Topic);
            if (existing == null) return false;
            await _documentRepository.FindOneAndReplaceAsync(
                e => string.CompareOrdinal(e.Topic, schema.Topic) == 0, schema);
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveSchema(string topic)
        {
            var existing = await GetSchema(topic);
            if (existing == null) return false;
            await _documentRepository.DeleteOneAsync(e => string.CompareOrdinal(e.Topic, topic) == 0);
            return true;
        }
    }
}