using Core;
using Microsoft.Extensions.Logging;
using System;

namespace Otel.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var serviceName = "ConsoleApp";
                FnLoggerFactory.Initialize(serviceName);
                FnTracerProvider.Initialize(serviceName);
                FnMeterProvider.Initialize(serviceName);

                var batch = new Batch();
                batch.Start();


            }
            finally
            {
                Console.ReadLine();
                FnTracerProvider.Dispose();
                FnMeterProvider.Dispose();
                FnLoggerFactory.Dispose();
            }

        }
    }

    internal class Batch
    {
        private ILogger Logger { get => FnLoggerFactory.GetLog<Batch>(); }

        public void Start()
        {
            Logger.LogInformation("batch start");
            var service = new BatchService();
            service.Call();
            Logger.LogInformation("batch end");

        }
    }
}
