﻿using KenticoCloud.Delivery.Tests.DependencyInjectionFrameworks.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KenticoCloud.Delivery.Tests.DependencyInjectionFrameworks
{
    [Collection("DI Tests")]
    public class SimpleInjectorTests
    {
        [Fact]
        public void DeliveryClientIsSuccessfullyResolvedFromSimpleInjectorContainer()
        {
            var container = DependencyInjectionFrameworksHelper
                .GetServiceCollection()
                .BuildSimpleInjectorServiceProvider();

            var client = (DeliveryClient) container.GetInstance<IDeliveryClient>();

            client.AssertDefaultDependencies();
        }

        [Fact]
        public void DeliveryClientIsSuccessfullyResolvedFromSimpleInjectorContainer_CustomModelProvider()
        {
            var container = DependencyInjectionFrameworksHelper
                .GetServiceCollection()
                .AddScoped<IModelProvider, FakeModelProvider>()
                .RegisterInlineContentItemResolvers()
                .BuildSimpleInjectorServiceProvider();

            var client = (DeliveryClient) container.GetInstance<IDeliveryClient>();

            client.AssertDefaultDependenciesWithModelProviderAndInlineContentItemTypeResolvers<FakeModelProvider>();
        }

        [Fact]
        public void FakeModelProviderIsSuccessfullyResolvedAfterCrossWireWithServiceCollection()
        {
            var container = DependencyInjectionFrameworksHelper
                .GetServiceCollection()
                .BuildSimpleInjectorServiceProvider();
            container.Register<IModelProvider, FakeModelProvider>();

            var resolvedService = container.GetService<IModelProvider>();

            Assert.IsType<FakeModelProvider>(resolvedService);
        }
    }
}
