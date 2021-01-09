using System;
using ApplicationInsightLogging.Data;
using ApplicationInsightLogging.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ApplicationInsightLogging.Business.Loggers
{
    public abstract class ConfigurationLogger : BaseLogger, ILogHelper
    {
        protected readonly Options _options;

        /// <summary>
        /// Sets the object up to log only whats in the options configuration section provided
        /// </summary>
        protected ConfigurationLogger()
        {
            _options ??= new Options() { LogErrors = true, LogInformation = true, LogWarnings = true };
        }

        /// <summary>
        /// Sets the object up to log only whats in the options configuration section provided
        /// </summary>
        /// <param name="optionsSettingName"></param>
        /// <param name="configuration"></param>
        protected ConfigurationLogger(string optionsSettingName, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(optionsSettingName))
            {
                throw new ArgumentNullException($"The {nameof(optionsSettingName)} is required");
            }

            var configuration1 = configuration ?? throw new ArgumentNullException($"The {nameof(configuration)} is required");

            //Attempts to pull the options section from the configuration and converts it into our options format
            _options = configuration1?.GetSection(optionsSettingName)?.Get<Options>();

            //Checks if we have been successful
            if (_options == null)
            {
                throw new ArgumentException($"The {nameof(configuration)} was not able to find any options");
            }
        }
    }
}