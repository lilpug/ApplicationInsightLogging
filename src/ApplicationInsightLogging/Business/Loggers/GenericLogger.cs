using System;
using System.Collections.Generic;
using ApplicationInsightLogging.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApplicationInsightLogging.Business.Loggers
{
    public class GenericLogger : ConfigurationLogger, ILogHelper
    {   
        private readonly ILogger _logger;        

        /// <summary>
        /// Sets the object to logs everything by default
        /// </summary>
        /// <param name="logger"></param>
        public GenericLogger(ILogger<GenericLogger> logger) : base()
        {
            _logger = logger ?? throw new ArgumentNullException($"The {nameof(logger)} is required");            
        }

        /// <summary>
        /// Sets the object to log only whats in the options configuration section provided
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="optionsSettingName"></param>
        /// <param name="configuration"></param>
        public GenericLogger(ILogger<GenericLogger> logger, string optionsSettingName, IConfiguration configuration) : base(optionsSettingName, configuration)
        {
            _logger = logger ?? throw new ArgumentNullException($"The {nameof(logger)} is required");
        }        

        /// <summary>
        /// Saves the information logs
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        protected override void SaveInformation(string name, Dictionary<string, string> parameters)
        {
            if (!_options.LogInformation.HasValue || _options.LogInformation.Value)
            {
                _logger.LogInformation($"{name}, {parameters?["Message"]}", parameters);                
            }
        }

        /// <summary>
        /// Saves the warning logs
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        protected override void SaveWarning(string name, Dictionary<string, string> parameters)
        {
            if (!_options.LogWarnings.HasValue || _options.LogWarnings.Value)
            {
                _logger.LogWarning($"{name}, {parameters?["Message"]}", parameters);                
            }
        }

        /// <summary>
        /// Saves the error logs
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        protected override void SaveError(Exception exception, Dictionary<string, string> parameters)
        {
            if (!_options.LogErrors.HasValue || _options.LogErrors.Value)
            {
                _logger.LogError(exception, parameters?["Message"], parameters);                
            }
        }
    }
}