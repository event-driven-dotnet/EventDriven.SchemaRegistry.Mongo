using System;
using EventDriven.SchemaRegistry.Abstractions;
using EventDriven.SchemaRegistry.Mongo;

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
            services.AddSingleton<ISchemaRegistry, MongoSchemaRegistry>();
            var schemaOptions = new MongoStateStoreOptions();
            configureStateStoreOptions.Invoke(schemaOptions);
            services.Configure(configureStateStoreOptions);
            return services;
        }
    }
}