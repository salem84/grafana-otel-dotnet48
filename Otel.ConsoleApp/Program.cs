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
                FnLoggerFactory.Initialize();
                FnTraceProvider.Initialize();
                var batch = new Batch();
                batch.Start();


            }
            finally
            {
                Console.ReadLine();
                FnTraceProvider.Dispose();
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
