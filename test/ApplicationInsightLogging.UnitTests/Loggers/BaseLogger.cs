using NUnit.Framework;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using ApplicationInsightLogging.Business.Loggers;
using ApplicationInsightLogging.Data.Interfaces;

namespace ApplicationInsightLogging.UnitTests.Loggers
{
    public class BaseLoggerTests
    {
        private Mock<BaseLogger> MockLogger { get; set; }

        [SetUp]
        public void Setup()
        {
            MockLogger = new Mock<BaseLogger>();
        }

        private ILogHelper CreateInstance()
        {
            return MockLogger.Object;
        }

        [Test]
        public void LogInformationMessage()
        {
            const string eventName = "TestingEventName";
            const string message = "this is a test message";

            var instance = CreateInstance();
            instance.LogInformation(eventName, message);

            MockLogger.Protected().Verify("SaveInformation", Times.Once(),
                "Information",
                ItExpr.Is<Dictionary<string, string>>(
                    d => d["EventName"] == eventName &&
                    d["Message"] == message &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogInformationMessageWithEventCode()
        {
            const string eventName = "TestingEventName";
            const string message = "this is a test message";
            const string code = "testcode1";

            var instance = CreateInstance();
            instance.LogInformation(eventName, message, code);

            MockLogger.Protected().Verify("SaveInformation", Times.Once(),
                "Information",
                ItExpr.Is<Dictionary<string, string>>(
                    d => d["EventName"] == eventName && 
                    d["Message"] == message &&
                    d["EventCode"] == code &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogInformationParameters()
        {   
            const string eventName = "TestingEventName";
            const string message = "this is a test message";
            const string paramOneName = "ExampleParam1";
            const string paramTwoName = "ExampleParam2";
            const string paramOneValue = "test1";
            const string paramTwoValue = "test2";

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { paramOneName, paramOneValue },
                { paramTwoName, paramTwoValue },
            };

            var instance = CreateInstance();
            instance.LogInformation(eventName, message, parameters);

            MockLogger.Protected().Verify("SaveInformation", Times.Once(),
                "Information",
                ItExpr.Is<Dictionary<string, string>>(
                    d => d[paramOneName] == paramOneValue &&
                    d[paramTwoName] == paramTwoValue &&
                    d["EventName"] == eventName && 
                    d["Message"] == message &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogInformationParametersWithEventCode()
        {
            const string eventName = "TestingEventName";
            const string message = "this is a test message";
            const string paramOneName = "ExampleParam1";
            const string paramTwoName = "ExampleParam2";
            const string paramOneValue = "test1";
            const string paramTwoValue = "test2";
            const string code = "testcode1";

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { paramOneName, paramOneValue },
                { paramTwoName, paramTwoValue },
            };

            var instance = CreateInstance();
            instance.LogInformation(eventName, message, parameters, code);

            MockLogger.Protected().Verify("SaveInformation", Times.Once(),
               "Information",
                ItExpr.Is<Dictionary<string, string>>(
                    d => d[paramOneName] == paramOneValue &&
                    d[paramTwoName] == paramTwoValue &&
                    d["EventName"] == eventName && 
                    d["Message"] == message &&
                    d["EventCode"] == code &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogWarningMessage()
        {
            const string message = "this is a test message";

            var instance = CreateInstance();
            instance.LogWarning(message);

            MockLogger.Protected().Verify("SaveWarning", Times.Once(), 
                "Warning", 
                ItExpr.Is<Dictionary<string, string>>(                    
                    d => d["Message"] == message &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogWarningMessageWithWarningCode()
        {
            const string message = "this is a test message";
            const string code = "testcode1";

            var instance = CreateInstance();
            instance.LogWarning(message, code);


            MockLogger.Protected().Verify("SaveWarning", Times.Once(),
                "Warning",
                ItExpr.Is<Dictionary<string, string>>(
                    d => d["Message"] == message &&
                    d["WarningCode"] == code &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogWarningWithParameters()
        {
            const string message = "this is a test message";
            const string paramOneName = "ExampleParam1";
            const string paramTwoName = "ExampleParam2";
            const string paramOneValue = "test1";
            const string paramTwoValue = "test2";            

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { paramOneName, paramOneValue },
                { paramTwoName, paramTwoValue },
            };

            var instance = CreateInstance();
            instance.LogWarning(message, parameters);

            MockLogger.Protected().Verify("SaveWarning", Times.Once(),
                "Warning",
                ItExpr.Is<Dictionary<string, string>>(
                    d => d[paramOneName] == paramOneValue &&
                    d[paramTwoName] == paramTwoValue &&
                    d["Message"] == message &&                    
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogWarningParametersWithWarningCode()
        {
            const string message = "this is a test message";            
            const string paramOneName = "ExampleParam1";
            const string paramTwoName = "ExampleParam2";
            const string paramOneValue = "test1";
            const string paramTwoValue = "test2";
            const string code = "testcode1";

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { paramOneName, paramOneValue },
                { paramTwoName, paramTwoValue },
            };

            var instance = CreateInstance();
            instance.LogWarning(message, parameters, code);

            MockLogger.Protected().Verify("SaveWarning", Times.Once(),
                "Warning",
                ItExpr.Is<Dictionary<string, string>>(
                    d => d[paramOneName] == paramOneValue &&
                    d[paramTwoName] == paramTwoValue &&
                    d["Message"] == message &&
                    d["WarningCode"] == code &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogErrorMessage()
        {
            var exception = new Exception("Testing");
            const string message = "this is a test message";

            var instance = CreateInstance();
            instance.LogError(message, exception);

            MockLogger.Protected().Verify("SaveError", Times.Once(),
                exception,
                ItExpr.Is<Dictionary<string, string>>(
                    d => d["Message"] == message &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogErrorMessageWithErrorCode()
        {
            var exception = new Exception("Testing");
            const string message = "this is a test message";
            const string code = "testcode1";

            var instance = CreateInstance();
            instance.LogError(message, exception, code);

            MockLogger.Protected().Verify("SaveError", Times.Once(),
                exception,
                ItExpr.Is<Dictionary<string, string>>(
                    d => d["Message"] == message &&
                    d["ErrorCode"] == code &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogErrorParameters()
        {
            var exception = new Exception("Testing");
            const string message = "this is a test message";
            const string paramOneName = "ExampleParam1";
            const string paramTwoName = "ExampleParam2";
            const string paramOneValue = "test1";
            const string paramTwoValue = "test2";

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { paramOneName, paramOneValue },
                { paramTwoName, paramTwoValue },
            };

            var instance = CreateInstance();
            instance.LogError(message, exception, parameters);

            MockLogger.Protected().Verify("SaveError", Times.Once(),
                exception,
                ItExpr.Is<Dictionary<string, string>>(
                    d => d[paramOneName] == paramOneValue &&
                    d[paramTwoName] == paramTwoValue &&
                    d["Message"] == message &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }

        [Test]
        public void LogErrorParametersWithErrorCode()
        {
            var exception = new Exception("Testing");
            const string message = "this is a test message";
            const string paramOneName = "ExampleParam1";
            const string paramTwoName = "ExampleParam2";
            const string paramOneValue = "test1";
            const string paramTwoValue = "test2";
            const string code = "testcode1";

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { paramOneName, paramOneValue },
                { paramTwoName, paramTwoValue },
            };

            var instance = CreateInstance();
            instance.LogError(message, exception, parameters, code);

            MockLogger.Protected().Verify("SaveError", Times.Once(),
                exception,
                ItExpr.Is<Dictionary<string, string>>(
                    d => d[paramOneName] == paramOneValue &&
                    d[paramTwoName] == paramTwoValue &&
                    d["ErrorCode"] == code &&
                    d["Message"] == message &&
                    d.ContainsKey("SpanId") &&
                    d.ContainsKey("ParentId") &&
                    d.ContainsKey("RequestId") &&
                    d.ContainsKey("TraceId") &&
                    d.ContainsKey("LogTimeStamp") &&
                    d.ContainsKey("LogMillisecondsStamp")
                    ));
        }
    }
}