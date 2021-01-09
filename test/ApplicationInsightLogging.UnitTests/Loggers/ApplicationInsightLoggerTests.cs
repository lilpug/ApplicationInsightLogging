using NUnit.Framework;
using Microsoft.ApplicationInsights;
using Moq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using ApplicationInsightLogging.Business.Loggers;
using Microsoft.Extensions.Configuration;

namespace ApplicationInsightLogging.UnitTests.Loggers
{
    public class ApplicationInsightLoggerTests
    {
        [Test]
        public void ConstructorNullTelemetryClient()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new ApplicationInsightLogger(null);
            });
            Assert.IsTrue(exception?.Message?.IndexOf("The telemetryClient is required") > -1);
        }

        [Test]
        public void DefaultOptions()
        {
            const string eventName = "testing event name";
            const string message = "testing message";
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            Exception exception = new Exception("testing exception");

            //Sets up the instance by using the mock channel in the telemetry configuration object
            var config = new TelemetryConfiguration("", mockTelemetryChannel.Object);
            var client = new TelemetryClient(config);
            var instance = new ApplicationInsightLogger(client);

            //Fires all three types to ensure the options are all defaulted to true
            instance.LogInformation(eventName, message);
            instance.LogWarning(message);
            instance.LogError(message, exception);

            mockTelemetryChannel.Verify(s => s.Send(It.IsAny<ITelemetry>()), Times.Exactly(3));
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

            //Sets up the instance by using the mock channel in the telemetry configuration object
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            var config = new TelemetryConfiguration("", mockTelemetryChannel.Object);
            var client = new TelemetryClient(config);
            var instance = new ApplicationInsightLogger(client, settingName, cfg);

            instance.LogInformation(eventName, message);
            
            mockTelemetryChannel.Verify(s => s.Send(It.IsAny<ITelemetry>()), Times.Never);
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

            //Sets up the instance by using the mock channel in the telemetry configuration object
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            var config = new TelemetryConfiguration("", mockTelemetryChannel.Object);
            var client = new TelemetryClient(config);
            var instance = new ApplicationInsightLogger(client, settingName, cfg);

            instance.LogWarning(message);

            mockTelemetryChannel.Verify(s => s.Send(It.IsAny<ITelemetry>()), Times.Never);
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

            //Sets up the instance by using the mock channel in the telemetry configuration object
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            var config = new TelemetryConfiguration("", mockTelemetryChannel.Object);
            var client = new TelemetryClient(config);
            var instance = new ApplicationInsightLogger(client, settingName, cfg);

            instance.LogError(message, exception);

            mockTelemetryChannel.Verify(s => s.Send(It.IsAny<ITelemetry>()), Times.Never);
        }

        [Test]
        public void SaveInformation()
        {
            const string eventName = "testing event name";
            const string message = "testing message";

            //Sets up the instance by using the mock channel in the telemetry configuration object
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            var config = new TelemetryConfiguration("", mockTelemetryChannel.Object);
            var client = new TelemetryClient(config);
            var instance = new ApplicationInsightLogger(client);

            //Logs some information
            instance.LogInformation(eventName, message);            

            mockTelemetryChannel.Verify(s => s.Send(It.IsAny<ITelemetry>()), Times.Once);
        }

        [Test]
        public void SaveWarning()
        {   
            const string message = "testing message";

            //Sets up the instance by using the mock channel in the telemetry configuration object
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            var config = new TelemetryConfiguration("", mockTelemetryChannel.Object);
            var client = new TelemetryClient(config);
            var instance = new ApplicationInsightLogger(client);

            //Logs a warning            
            instance.LogWarning(message);

            mockTelemetryChannel.Verify(s => s.Send(It.IsAny<ITelemetry>()), Times.Once);
        }

        [Test]
        public void SaveError()
        {   
            const string message = "testing message";
            Exception exception = new Exception("testing exception");

            //Sets up the instance by using the mock channel in the telemetry configuration object
            var mockTelemetryChannel = new Mock<ITelemetryChannel>();
            var config = new TelemetryConfiguration("", mockTelemetryChannel.Object);
            var client = new TelemetryClient(config);
            var instance = new ApplicationInsightLogger(client);

            //Logs an error
            instance.LogError(message, exception);
            
            mockTelemetryChannel.Verify(s => s.Send(It.IsAny<ITelemetry>()), Times.Once);
        }        
    }
}