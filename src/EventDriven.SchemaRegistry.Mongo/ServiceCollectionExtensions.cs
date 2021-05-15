using System;
using EventDriven.SchemaRegistry.Abstractions;
using EventDriven.SchemaRegistry.Mongo;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using URF.Core.Abstractions;
using URF.Core.Mongo;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds MongoSchemaRegistry services to the provided <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /></param>
        /// <param name="configureStateStoreOptions">Configure state store settings</param>
        /// <returns>The original <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</returns>
        public static IServiceCollection AddMongoSchemaRegistry(this IServiceCollection services,
            Action<MongoStateStoreOptions> configureStateStoreOptions)
        {
            // Configure options
            var schemaOptions = new MongoStateStoreOptions();
            configureStateStoreOptions.Invoke(schemaOptions);
            services.Configure(configureStateStoreOptions);

            // Register Mongo database and schema collection
            services.AddSingleton<IMongoDatabase>(_ =>
            {
                var client = new MongoClient(schemaOptions.ConnectionString);
                return client.GetDatabase(schemaOptions.DatabaseName);
            });
            services.AddSingleton<IMongoCollection<MongoSchema>>(sp =>
            {
                var context = sp.GetRequiredService<IMongoDatabase>();
                return context.GetCollection<MongoSchema>(schemaOptions.SchemasCollectionName);
            });
            
            // Register services
            services.AddSingleton<ISchemaRegistry, MongoSchemaRegistry>();
            services.AddSingleton<IDocumentRepository<MongoSchema>, DocumentRepository<MongoSchema>>();

            // Register caml case convention
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention()
            };
            ConventionRegistry.Register("camlCase", conventionPack, _ => true);

            return services;
        }
    }
}