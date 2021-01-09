using ApplicationInsightLogging.Business.StartupConfiguration;
using ApplicationInsightLogging.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationInsightLogging.Business.Loggers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ApplicationInsightLogging.UnitTests.StartupConfiguration
{
    public class ServicesConfigurationTests
    {
        private ServiceCollection ServiceCollection { get; set; }
        
        private IConfiguration Configuration { get; set; }
        
        private string SettingsName { get; set; }

        [SetUp]
        public void Setup()
        {
            ServiceCollection = new ServiceCollection();
            
            SettingsName = "testingOptions";
            var settings = new Dictionary<string, string>()
            {
                { $"{SettingsName}:LogInformation", "false" },
                { $"{SettingsName}:LogWarnings", "false" },
                { $"{SettingsName}:LogErrors", "false" }
            };
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            Configuration = cfgBuilder.Build();
        }

        [Test]
        public void AddDevelopmentApplicationInsightLogging()
        {   
            ServiceCollection.AddScoped<ILoggerFactory, NullLoggerFactory>();
            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), false);
            ServiceCollection.AddDevelopmentApplicationInsightLogging();
            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), true);

            IServiceProvider provider = ServiceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService(typeof(ILogHelper));
            
            Assert.AreEqual(typeof(GenericLogger), service?.GetType());
        }

        [Test]
        public void AddDevelopmentApplicationInsightLoggingWithSettings()
        {
            ServiceCollection.AddScoped<ILoggerFactory, NullLoggerFactory>();
            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), false);
            ServiceCollection.AddDevelopmentApplicationInsightLogging(SettingsName, Configuration);
            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), true);
            
            IServiceProvider provider = ServiceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService(typeof(ILogHelper));
            
            Assert.AreEqual(typeof(GenericLogger), service?.GetType());
        }

        [Test]
        public void AddApplicationInsightLogging()
        {
            const string instrumentationKey = "testing";
            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), false);
            ServiceCollection.AddApplicationInsightLogging(instrumentationKey);
            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), true);
            
            IServiceProvider provider = ServiceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService(typeof(ILogHelper));
            
            Assert.AreEqual(typeof(ApplicationInsightLogger), service?.GetType());
        }

        [Test]
        public void AddApplicationInsightLoggingWithSettings()
        {
            const string instrumentationKey = "testing";

            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), false);
            ServiceCollection.AddApplicationInsightLogging(instrumentationKey, SettingsName, Configuration);
            Assert.AreEqual(ServiceCollection.Any(x => x.ServiceType == typeof(ILogHelper)), true);
            
            IServiceProvider provider = ServiceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService(typeof(ILogHelper));
            
            Assert.AreEqual(typeof(ApplicationInsightLogger), service?.GetType());
        }
    }
}
