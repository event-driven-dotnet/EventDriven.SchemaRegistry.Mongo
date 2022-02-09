using System;
using EventDriven.DependencyInjection.URF.Mongo;
using EventDriven.SchemaRegistry.Abstractions;
using EventDriven.SchemaRegistry.Mongo;
using Microsoft.Extensions.Configuration;
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
        /// <param name="configuration">The application's <see cref="IConfiguration"/>.</param>
        /// <returns>The original <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</returns>
        public static IServiceCollection AddMongoSchemaRegistry(this IServiceCollection services, IConfiguration configuration)
        {
            // Add MongoDb settings
            services.AddMongoDbSettings<MongoStateStoreOptions, MongoSchema>(configuration);

            // Register services
            services.AddSingleton<ISchemaRegistry, MongoSchemaRegistry>();
            services.AddSingleton<IDocumentRepository<MongoSchema>, DocumentRepository<MongoSchema>>();

            return services;
        }

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

            // Register IMongoCollection<MongoSchema>
            services.AddSingleton(_ =>
            {
                var context = new MongoSchemaRegistryDbContext(new MongoClient(schemaOptions.ConnectionString),
                    schemaOptions.DatabaseName, schemaOptions.CollectionName);
                return context.MongoSchemas;
            });
            
            // Register services
            services.AddSingleton<ISchemaRegistry, MongoSchemaRegistry>();
            services.AddSingleton<IDocumentRepository<MongoSchema>, DocumentRepository<MongoSchema>>();

            return services;
        }
    }
}