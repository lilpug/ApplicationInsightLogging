using NUnit.Framework;
using System;
using System.Collections.Generic;
using ApplicationInsightLogging.Business.Loggers;
using Microsoft.Extensions.Configuration;
using ApplicationInsightLogging.Data;

namespace ApplicationInsightLogging.UnitTests.Loggers
{
    public class ConfigurationLoggerTests
    {
        private class MockLogger : ConfigurationLogger
        {
            public MockLogger() : base()
            { }

            public MockLogger(string optionsSettingName, IConfiguration configuration) : base(optionsSettingName, configuration)
            { }

            public Options GetOptions()
            {
                return _options;
            }

            protected override void SaveError(Exception exception, Dictionary<string, string> parameters)
            {
                throw new NotImplementedException();
            }

            protected override void SaveInformation(string name, Dictionary<string, string> parameters)
            {
                throw new NotImplementedException();
            }

            protected override void SaveWarning(string name, Dictionary<string, string> parameters)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void ConstructorDefault()
        {
            var instance = new MockLogger();
            var results = instance.GetOptions();
            Assert.AreEqual(results?.LogErrors, true);
            Assert.AreEqual(results?.LogInformation, true);
            Assert.AreEqual(results?.LogWarnings, true);
        }

        [Test]
        public void ConstructorNullOptionsSettingName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new MockLogger(null, null);
            });
            Assert.IsTrue(exception?.Message?.IndexOf("The optionsSettingName is required") > -1);
        }

        [Test]
        public void ConstructorEmptyOptionsSettingName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new MockLogger("", null);
            });
            Assert.IsTrue(exception?.Message?.IndexOf("The optionsSettingName is required") > -1);
        }

        [Test]
        public void ConstructorNullConfiguration()
        {
            const string settingName = "test setting name";
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new MockLogger(settingName, null);
            });
            Assert.IsTrue(exception?.Message?.IndexOf("The configuration is required") > -1);
        }

        [Test]
        public void ConstructorNoOptionsFound()
        {
            const string settingName = "test setting name";
            Dictionary<string, string> settings = new Dictionary<string, string>();            
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            IConfiguration cfg = cfgBuilder.Build();

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var instance = new MockLogger(settingName, cfg);
            });
            Assert.IsTrue(exception?.Message?.IndexOf("The configuration was not able to find any options") > -1);
        }

        [Test]
        public void LogInformationOption()
        {
            const bool logInformation = true;
            const string settingName = "testingOptions";
            Dictionary<string, string> settings = new Dictionary<string, string>()
            {
                { $"{settingName}:LogInformation", logInformation.ToString() }
            };
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            IConfiguration cfg = cfgBuilder.Build();

            var instance = new MockLogger(settingName, cfg);            
            var results = instance.GetOptions();
            Assert.AreEqual(results?.LogErrors, null);
            Assert.AreEqual(results?.LogInformation, logInformation);
            Assert.AreEqual(results?.LogWarnings, null);
        }

        [Test]
        public void LogWarningsOption()
        {
            const bool logWarning = true;
            const string settingName = "testingOptions";
            Dictionary<string, string> settings = new Dictionary<string, string>()
            {
                { $"{settingName}:LogWarnings", logWarning.ToString() }
            };
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            IConfiguration cfg = cfgBuilder.Build();

            var instance = new MockLogger(settingName, cfg);
            var results = instance.GetOptions();
            Assert.AreEqual(results?.LogErrors, null);
            Assert.AreEqual(results?.LogInformation, null);
            Assert.AreEqual(results?.LogWarnings, logWarning);
        }

        [Test]
        public void LogErrorsOption()
        {
            const bool logError = true;
            const string settingName = "testingOptions";
            Dictionary<string, string> settings = new Dictionary<string, string>()
            {
                { $"{settingName}:LogErrors", logError.ToString() }
            };
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);
            IConfiguration cfg = cfgBuilder.Build();

            var instance = new MockLogger(settingName, cfg);
            var results = instance.GetOptions();
            Assert.AreEqual(results?.LogErrors, logError);
            Assert.AreEqual(results?.LogInformation, null);
            Assert.AreEqual(results?.LogWarnings, null);            
        }  
    }
}