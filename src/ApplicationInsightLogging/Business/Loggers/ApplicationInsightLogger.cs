using System;
using System.Collections.Generic;
using ApplicationInsightLogging.Data.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;

namespace ApplicationInsightLogging.Business.Loggers
{
    public class ApplicationInsightLogger : ConfigurationLogger, ILogHelper
    {   
        private readonly TelemetryClient _telemetryClient;
        
        /// <summary>
        /// Sets the object up to logs everything by default
        /// </summary>
        /// <param name="telemetryClient"></param>
        public ApplicationInsightLogger(TelemetryClient telemetryClient) : base()
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException($"The {nameof(telemetryClient)} is required");            
        }

        /// <summary>
        /// Sets the object up to log only whats in the options configuration section provided
        /// </summary>
        /// <param name="telemetryClient"></param>
        /// <param name="optionsSettingName"></param>
        /// <param name="configuration"></param>
        public ApplicationInsightLogger(TelemetryClient telemetryClient, string optionsSettingName, IConfiguration configuration)
            : base(optionsSettingName, configuration)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException($"The {nameof(telemetryClient)} is required");
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
                _telemetryClient.TrackEvent(name, parameters);
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
                _telemetryClient.TrackEvent(name, parameters);
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
                _telemetryClient.TrackException(exception, parameters);
            }
        }
    }
}