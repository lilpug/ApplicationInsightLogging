using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ApplicationInsightLogging.Data.Interfaces;

namespace ApplicationInsightLogging.Business.Loggers
{
    public abstract class BaseLogger : ILogHelper
    {
        private readonly Dictionary<string, string> _genericParameters = new()
        {
            { "SpanId", Activity.Current?.SpanId.ToString() },
            { "ParentId", Activity.Current?.ParentSpanId.ToString() },
            { "RequestId", Activity.Current?.Id },
            { "TraceId", Activity.Current?.TraceId.ToString() },
            { "LogTimeStamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
            { "LogMillisecondsStamp",  new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString() }
        }; 
        
        private Dictionary<K, V> Merge<K, V>(params Dictionary<K, V>[] dictionaries)
        {
            return dictionaries.SelectMany(x => x)
                .ToDictionary(x => x.Key, y => y.Value);
        }
        
        protected abstract void SaveInformation(string name, Dictionary<string, string> parameters);

        protected abstract void SaveWarning(string name, Dictionary<string, string> parameters);

        protected abstract void SaveError(Exception exception, Dictionary<string, string> parameters);

        /// <summary>
        /// Logs some information
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        public void LogInformation(string eventName, string message)
        {
            var parameters = new Dictionary<string, string>
            {
                { "EventName", eventName },
                { "Message", message },
            };
            
            SaveInformation("Information", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs some information with an event code to support it in the log
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <param name="eventCode"></param>
        public void LogInformation(string eventName, string message, string eventCode)
        {
            var parameters = new Dictionary<string, string>
            {
                { "EventName", eventName },
                { "EventCode", eventCode },
                { "Message", message },
            };
            SaveInformation("Information", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs some information with any supplied parameters to support it in the log
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public void LogInformation(string eventName, string message, Dictionary<string, string> parameters)
        {
            parameters ??= new Dictionary<string, string>();
            parameters.Add("EventName", eventName);
            parameters.Add("Message", message);
            SaveInformation("Information", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs some information with any supplied parameters and an event code to support it in the log
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <param name="eventCode"></param>
        public void LogInformation(string eventName, string message, Dictionary<string, string> parameters, string eventCode)
        {
            parameters ??= new Dictionary<string, string>();
            parameters.Add("EventName", eventName);
            parameters.Add("EventCode", eventCode);
            parameters.Add("Message", message);
            SaveInformation("Information", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            var parameters = new Dictionary<string, string>
            {                
                { "Message", message }
            };
            SaveWarning("Warning", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs a warning with a warning code to support it in the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="warningCode"></param>
        public void LogWarning(string message, string warningCode)
        {
            var parameters = new Dictionary<string, string>
            {
                { "WarningCode", warningCode },
                { "Message", message },
            };
            SaveWarning("Warning", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs a warning with any supplied parameters to support it in the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public void LogWarning(string message, Dictionary<string, string> parameters)
        {
            parameters ??= new Dictionary<string, string>();
            parameters.Add("Message", message);
            SaveWarning("Warning", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs a warning with any supplied parameters and a warning code to support it in the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <param name="warningCode"></param>
        public void LogWarning(string message, Dictionary<string, string> parameters, string warningCode)
        {
            parameters ??= new Dictionary<string, string>();
            parameters.Add("WarningCode", warningCode);
            parameters.Add("Message", message);
            SaveWarning("Warning", Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void LogError(string message, Exception exception)
        {
            var parameters = new Dictionary<string, string>
            {   
                { "Message", message },
            };
            SaveError(exception, Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs an error with an error code to support it in the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="errorCode"></param>
        public void LogError(string message, Exception exception, string errorCode)
        {
            var parameters = new Dictionary<string, string>
            {
                { "ErrorCode", errorCode },
                { "Message", message },
            };
            SaveError(exception, Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs an error with any supplied parameters to support it in the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        public void LogError(string message, Exception exception, Dictionary<string, string> parameters)
        {
            parameters ??= new Dictionary<string, string>();            
            parameters.Add("Message", message);
            SaveError(exception, Merge(_genericParameters, parameters));
        }

        /// <summary>
        /// Logs an error with any supplied parameters and an error code to support it in the log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        /// <param name="errorCode"></param>
        public void LogError(string message, Exception exception, Dictionary<string, string> parameters, string errorCode)
        {
            parameters ??= new Dictionary<string, string>();
            parameters.Add("ErrorCode", errorCode);
            parameters.Add("Message", message);
            SaveError(exception, Merge(_genericParameters, parameters));
        }
    }
}