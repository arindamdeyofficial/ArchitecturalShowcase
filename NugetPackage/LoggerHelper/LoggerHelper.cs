using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.AzureBlobStorage;
using Serilog.Sinks.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using System;
using SecretsKeyVault;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace CustomLoggerHelper
{
    public class LoggerHelper : ILoggerHelper
    {
        public readonly ILogger _logger;
        private IConfigurationRoot _configRoot;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;

        private string _sqlConStr;
        private string _logFilePath;
        private string _mongoConnectionString;
        private string _mongoCollectionName;
        private string _appInsightskey;

        // Constructor that accepts IConfigurationRoot
        public LoggerHelper(IConfigurationRoot configuration, IKeyVaultManagedIdentityHelper secretsHelper)
        {
            _configRoot = configuration;
            _secretsHelper = secretsHelper;

            ReadSettings(configuration).GetAwaiter().GetResult();
            // SQL Sink Column Options
            var sqlSinkOptions = new ColumnOptions
            {
                TimeStamp = { ColumnName = "Timestamp" },
                Message = { ColumnName = "Message" },
                Level = { ColumnName = "LogLevel" },
                Exception = { ColumnName = "Exception" }
            };

            // Configure Serilog with multiple sinks
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()  // Add context information to logs (optional)
                .WriteTo.Console()  // Log to console using Serilog.Sinks.Console
                .WriteTo.File(_logFilePath, rollingInterval: RollingInterval.Day)  // Log to a flat file with rolling
                .WriteTo.MSSqlServer(_sqlConStr, tableName: "Logs", autoCreateSqlTable: true, columnOptions: sqlSinkOptions)  // Log to SQL Server
                                                                                                                              //.WriteTo.AzureBlobStorage(
                                                                                                                              //    storageConnectionString,   // Connection string to Azure Blob Storage
                                                                                                                              //    blobContainerName,         // The blob container name
                                                                                                                              //    restrictedToMinimumLevel: LogEventLevel.Information)  // Minimum log level
                .WriteTo.MongoDB(
                    _mongoConnectionString,
                    _mongoCollectionName,
                    restrictedToMinimumLevel: LogEventLevel.Information)  // Log to MongoDB
                .WriteTo.ApplicationInsights(_appInsightskey, TelemetryConverter.Traces)  // Log to Application Insights
                .WriteTo.Trace()  // Log to system trace
                .MinimumLevel.Information()  // Set the minimum log level for all sinks
                .CreateLogger();

            _logger = Log.Logger;  // Store the configured logger
            _secretsHelper = secretsHelper;
        }
        public async Task ReadSettings(IConfigurationRoot configuration)
        {
            // Retrieve configuration settings
            _sqlConStr = await _secretsHelper.GetSecretAsync("LogSqlConnection");
            _logFilePath = await _secretsHelper.GetSecretAsync("LogFilePath");
            _mongoConnectionString = await _secretsHelper.GetSecretAsync("LogMongoDb");
            _mongoCollectionName = await _secretsHelper.GetSecretAsync("LogMongoCollectionName");
            var appInsightsInstrumentationKeys = await _secretsHelper.GetSecretAsync("AppInsightsInstrumentationKeys");
            JObject jsonObject = JObject.Parse(appInsightsInstrumentationKeys);
            _appInsightskey = jsonObject[_configRoot["ApiApplicationName"]].ToString();
        }

        // Method to log errors
        public void LogError(Exception ex, string msg)
        {
            if (ex != null)
            {
                _logger.Error(ex, msg);
            }
            else
            {
                _logger.Error("An unknown error occurred.");
            }
        }
        public void LogError(string msg)
        {
            _logger.Error(msg);
        }

        // Method to log general information
        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        // Method to log warnings
        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        // Method to log critical issues
        public void LogCritical(string message)
        {
            _logger.Fatal(message);
        }

        // Close and flush the logger when the application shuts down
        public void CloseLogger()
        {
            Log.CloseAndFlush();
        }
    }
}
