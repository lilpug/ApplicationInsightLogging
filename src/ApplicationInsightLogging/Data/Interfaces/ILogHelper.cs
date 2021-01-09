using System;
using System.Collections.Generic;

namespace ApplicationInsightLogging.Data.Interfaces
{
    public interface ILogHelper
    {
        void LogInformation(string eventName, string message);
        void LogInformation(string eventName, string message, string eventCode);
        void LogInformation(string eventName, string message, Dictionary<string, string> parameters);
        void LogInformation(string eventName, string message, Dictionary<string, string> parameters, string eventCode);
        void LogWarning(string message);
        void LogWarning(string message, string warningCode);
        void LogWarning(string message, Dictionary<string, string> parameters);
        void LogWarning(string message, Dictionary<string, string> parameters, string warningCode);
        void LogError(string message, Exception exception);
        void LogError(string message, Exception exception, string errorCode);
        void LogError(string message, Exception exception, Dictionary<string, string> parameters);
        void LogError(string message, Exception exception, Dictionary<string, string> parameters, string errorCode);
    }
}
