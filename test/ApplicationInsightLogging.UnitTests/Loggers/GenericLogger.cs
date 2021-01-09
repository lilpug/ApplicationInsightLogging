using NUnit.Framework;
using Moq;
using Microsoft.ApplicationInsights.Channel;
using System;
using System.Collections.Generic;
using ApplicationInsightLogging.Business.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ApplicationInsightLogging.Data.Interfaces;

namespace ApplicationInsightLogging.UnitTests.Loggers
{
    public class GenericLoggerTests
    {
        private Mock<ILogger<GenericLogger>> MockLogger { get; set; }

        [SetUp]
        public void Setup()
        {
            MockLogger = new Mock<ILogger<GenericLogger>>();
        }

        private ILogHelper CreateInstance()
        {
            return new GenericLogger(MockLogger.Object);
        }

        private ILogHelper CreateInstance(string settingName, IConfiguration configuration)
        {   
            return new GenericLogger(MockLogger.Object, settingName, configuration);
        }

        [Test]
        public void DefaultOptions()
        {
            const string eventName = "testing event name";
            const string message = "testing message";
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            Exception exception = new Exception("testing exception");

            var instance = CreateInstance();
            
            //Fires all three types to ensure the options are all defaulted to true
            instance.LogInformation(eventName, message);
            instance.LogWarning(message);
            instance.LogError(message, exception);

            MockLogger.Verify(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), 
              Times.Exactly(3));
        }
        
        [Test]
        public void LogInformationOption()
        {
            const string settingName = "testingOptions";
            Dictionary<string, string> settings = new Dictionary<string, string>()
            {
                { $"{settingName}:LogInformation", "false" }
            };
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            IConfiguration cfg = cfgBuilder.Build();

            const string eventName = "testing event name";
            const string message = "testing message";

            var instance = CreateInstance(settingName, cfg);

            //Fires all three types to ensure the options are all defaulted to true
            instance.LogInformation(eventName, message);            

            MockLogger.Verify(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
              Times.Never);
        }

        [Test]
        public void LogWarningsOption()
        {
            const string settingName = "testingOptions";
            Dictionary<string, string> settings = new Dictionary<string, string>()
            {
                { $"{settingName}:LogWarnings", "false" }
            };
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            IConfiguration cfg = cfgBuilder.Build();

            const string message = "testing message";

            var instance = CreateInstance(settingName, cfg);

            //Fires all three types to ensure the options are all defaulted to true
            instance.LogWarning(message);

            MockLogger.Verify(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
              Times.Never);
        }

        [Test]
        public void LogErrorsOption()
        {
            const string settingName = "testingOptions";
            Dictionary<string, string> settings = new Dictionary<string, string>()
            {
                { $"{settingName}:LogErrors", "false" }
            };
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            IConfiguration cfg = cfgBuilder.Build();

            const string message = "testing message";
            Exception exception = new Exception("testing exception");

            var instance = CreateInstance(settingName, cfg);

            //Fires all three types to ensure the options are all defaulted to true
            instance.LogError(message, exception);

            MockLogger.Verify(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
              Times.Never);
        }

        [Test]
        public void SaveInformation()
        {
            const string eventName = "testing event name";
            const string message = "testing message";

            var instance = CreateInstance();

            //Fires all three types to ensure the options are all defaulted to true
            instance.LogInformation(eventName, message);

            MockLogger.Verify(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
              Times.Once);
        }

        [Test]
        public void SaveWarning()
        {   
            const string message = "testing message";

            var instance = CreateInstance();

            //Fires all three types to ensure the options are all defaulted to true
            instance.LogWarning(message);

            MockLogger.Verify(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
              Times.Once);
        }

        [Test]
        public void SaveError()
        {   
            const string message = "testing message";
            Exception exception = new Exception("testing exception");

            var instance = CreateInstance();

            //Fires all three types to ensure the options are all defaulted to true
            instance.LogError(message, exception);

            MockLogger.Verify(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
              Times.Once);
        }        
    }
}