# ApplicationInsightLogging

ApplicationInsightLogging is a small library that writes logs and exceptions to Application Insights.

[![NuGet](https://img.shields.io/nuget/v/ApplicationInsightLogging.svg?maxAge=3600)](https://www.nuget.org/packages/ApplicationInsightLogging/1.0.0)

## Getting Started

1. Install the library via its NuGet package.

2. Add the 'services.AddApplicationInsightLogging' type to the Startup.cs service registration section.
  
3. Inject an ILogHelper wherever you would like to use it.

## Log Types

This library allows you to log information, warning and error events.

It stores the information and warning logs under custom events in application insights, which can then be filtered by name using either 'Information' or 'Warning'. 

## Options Settings

When registering the library for dependency injection, there are two versions of the constructor. When using the SettingName and IConfiguration version, it allows you to either enable/disable individual logging elements based on a configuration setup.

### SettingsName
The SettingsName parameter is used to tell the IConfiguration what parent element it should be looking for in the configuration.

### Configuration
The Configuration parameter is used to extract the settings and serialize them into an internal options object, that gets checked internally before firing each logging function.

### Example Settings structure
Below is the structure the configuration will need to be in, (this could be from the appsettings.json file).


```json
{
	"SettingsNameParameterValue": {
		"LogInformation": true,
		"LogWarnings": true,
		"LogErrors": true
	}
}
```

## Default Custom Dimensions

When using any of the logging functions, the below is always injected into the Custom Dimensions, along with any parameters that have been supplied to the functions.

- SpanId
- ParentId
- RequestId
- TraceId
- LogTimeStamp (utc)
- LogMillisecondsStamp (utc)

## Development

If you are working locally and would prefer not to send data to application insights, you can either use the configuration settings version to turn off the logger or you can use the 'GenericLogger' to replace the 'ApplicationInsightLogger'. This can be done by using 'AddDevelopmentApplicationInsightLogging' at the service registration instead of 'AddApplicationInsightLogging'.