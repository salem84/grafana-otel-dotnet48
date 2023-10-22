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

        public static void Initialize()
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            _loggerFactory = LoggerFactory.Create(builder => builder
                    //.AddConsole()
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
    }
}
