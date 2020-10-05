﻿using System;
using Kentico.Kontent.Delivery.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kentico.Kontent.Delivery.Caching.Extensions
{
    /// <summary>
    /// A class which contains extension methods on <see cref="IServiceCollection"/> for registering an cached <see cref="IDeliveryClient"/> instance.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a delegate that will be used to configure a cached <see cref="IDeliveryClient"/>.
        /// </summary>
        /// <param name="services">A <see cref="IServiceCollection"/> instance for registering and resolving dependencies.</param>
        /// <param name="options">A <see cref="DeliveryCacheOptions"/> instance.</param>
        /// <returns>The <paramref name="services"/> instance with cache services registered in it</returns>
        public static IServiceCollection AddDeliveryClientCache(this IServiceCollection services, DeliveryCacheOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "The Delivery cache  options object is not specified.");
            }

            return services
                 .RegisterCacheOptions(options)
                 .RegisterDependencies(options.CacheType)
                 .Decorate<IDeliveryClient, DeliveryClientCache>();
        }

        /// <summary>
        ///  Registers a delegate that will be used to configure a cached <see cref="IDeliveryClient"/>.
        /// </summary>
        /// <param name="services">A <see cref="IServiceCollection"/> instance for registering and resolving dependencies.</param>
        /// <param name="name">A name of named client which want to use cached <see cref="IDeliveryClient"/></param>
        /// <param name="options">A <see cref="DeliveryCacheOptions"/> instance. </param> 
        /// <returns>The <paramref name="services"/> instance with cache services registered in it</returns>
        public static IServiceCollection AddDeliveryClientCache(this IServiceCollection services, string name, DeliveryCacheOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "The Delivery cache  options object is not specified.");
            }

            return services
                .RegisterCacheOptions(options, name)
                .RegisterDependencies(options.CacheType, name)
                .Decorate<IDeliveryClientFactory, DeliveryClientCacheFactory>();
        }

        private static IServiceCollection RegisterCacheOptions(this IServiceCollection services, DeliveryCacheOptions options, string name = null)
        {
            void Configure(DeliveryCacheOptions o)
            {
                o.DefaultExpiration = options.DefaultExpiration;
                o.StaleContentExpiration = options.StaleContentExpiration;
                o.CacheType = options.CacheType;
                o.Name = name;
            }
            if (name == null)
            {
                services.Configure<DeliveryCacheOptions>(Configure);
            }
            else
            {
                services.Configure<DeliveryCacheOptions>(name, Configure);
            }

            return services;
        }

        private static IServiceCollection RegisterDependencies(this IServiceCollection services, CacheTypeEnum cacheType, string name = null)
        {
            switch (cacheType)
            {
                case CacheTypeEnum.Memory:
                    if (name == null)
                    {
                        services.TryAddSingleton<IDeliveryCacheManager, MemoryCacheManager>();
                    }
                    services.TryAddSingleton<IMemoryCache, MemoryCache>();
                    break;

                case CacheTypeEnum.Distributed:
                    if (name == null)
                    {
                        services.TryAddSingleton<IDeliveryCacheManager, DistributedCacheManager>();
                    }
                    services.TryAddSingleton<IDistributedCache, MemoryDistributedCache>();
                    break;
            }

            return services;
        }
    }
}
