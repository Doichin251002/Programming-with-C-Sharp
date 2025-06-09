using BookStore.DL.Cache;
using BookStore.DL.Interfaces;
using BookStore.DL.Kafka;
using BookStore.DL.Kafka.KafkaCache;
using BookStore.DL.Repositories;
using BookStore.Models.Configurations;
using BookStore.Models.POCO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.DL
{
    public static class DependencyInjection
    {
        public static IServiceCollection
            AddDataDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<IAuthorRepository, AuthorRepository>();
            services.AddSingleton<IStoreLocationGateway, StoreLocationGateway>();

            services.AddCache<BookCacheConfiguration, BookRepository, Book, string>(config);

            services.AddHostedService<KafkaCache<string, Book>>();

            return services;
        }

  
        public static IServiceCollection AddCache<TCacheConfiguration, TCacheRepository, TData, TKey>(this IServiceCollection services, IConfiguration config)
           where TCacheConfiguration : CacheConfiguration
           where TCacheRepository : class, ICacheRepository<TKey, TData>
           where TData : ICacheItem<TKey>
           where TKey : notnull
        {
            var configSection = config.GetSection(typeof(TCacheConfiguration).Name);

            if (!configSection.Exists())
            {
                throw new ArgumentNullException(typeof(TCacheConfiguration).Name, "Configuration section is missing in appsettings!");
            }

            services.Configure<TCacheConfiguration>(configSection);

            services.AddSingleton<ICacheRepository<TKey, TData>, TCacheRepository>();
            services.AddSingleton<IKafkaProducer<TData>, KafkaProducer<TKey, TData, TCacheConfiguration>>();
            services.AddHostedService<MongoCachePopulator<TData, TCacheConfiguration, TKey>>();

            return services;
        }
    }
}
