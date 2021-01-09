using ApplicationInsightLogging.Business.Loggers;
using ApplicationInsightLogging.Data.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace ApplicationInsightLogging.Business.StartupConfiguration
{
    public static class ServicesConfiguration
    {
        public static void AddDevelopmentApplicationInsightLogging(this IServiceCollection services)
        {
            services.TryAddScoped(typeof(ILogHelper), serviceProvider =>
            {
                var service = serviceProvider.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                return new GenericLogger(service.CreateLogger<GenericLogger>());
            });
        }

        public static void AddDevelopmentApplicationInsightLogging(this IServiceCollection services, string settingsName, IConfiguration configuration)
        {
            services.TryAddScoped(typeof(ILogHelper), serviceProvider =>
            {
                var service = serviceProvider.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                return new GenericLogger(service.CreateLogger<GenericLogger>(), settingsName, configuration);
            });
        }

        public static void AddApplicationInsightLogging(this IServiceCollection services, string instrumentationKey)
        {
            services.TryAddScoped(typeof(ILogHelper), _ =>
            {
                var options = new TelemetryConfiguration(instrumentationKey);
                var client = new TelemetryClient(options);
                return new ApplicationInsightLogger(client);
            });
        }

        public static void AddApplicationInsightLogging(this IServiceCollection services, string instrumentationKey, string settingsName, IConfiguration configuration)
        {
            services.TryAddScoped(typeof(ILogHelper), _ =>
            {
                var options = new TelemetryConfiguration(instrumentationKey);
                var client = new TelemetryClient(options);
                return new ApplicationInsightLogger(client, settingsName, configuration);
            });
        }
    }
}