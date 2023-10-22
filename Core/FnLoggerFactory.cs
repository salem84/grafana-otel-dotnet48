using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Concurrent;

namespace Core
{
    public static class FnLoggerFactory
    {
        private static ILoggerFactory _loggerFactory;

        private static ConcurrentDictionary<Type, Microsoft.Extensions.Logging.ILogger> loggerByType
            = new ConcurrentDictionary<Type, Microsoft.Extensions.Logging.ILogger>();

        public static void Initialize(ILoggerFactory loggerFactory)
        {
            if (_loggerFactory != null)
                throw new InvalidOperationException("FnLoggerFactory already initialized!");

            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public static void Initialize(string serviceName)
        {
            // Serilog.Debugging.SelfLog.Enable(Console.Out);

            var serilogLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = "http://localhost:4318/v1/logs";
                    options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf; // disabilitato GRPC perchè richiede su .NET 4.8 una configurazione aggiuntiva
                    options.ResourceAttributes["service.name"] = serviceName;


                })
                .CreateLogger();

            _loggerFactory = LoggerFactory.Create(builder => builder
                    //.AddConsole() // Standard Log Console
                    .AddSerilog(serilogLogger)
                    );
        }

        public static Microsoft.Extensions.Logging.ILogger GetLog<T>()
        {
            if (_loggerFactory is null)
                throw new InvalidOperationException("FnLoggerFactory is not initialized yet.");

            return loggerByType
                .GetOrAdd(typeof(T), _loggerFactory.CreateLogger<T>());
        }

        public static void Dispose()
        {
            if (_loggerFactory != null)
            {
                _loggerFactory.Dispose();
            }
        }
    }
}
