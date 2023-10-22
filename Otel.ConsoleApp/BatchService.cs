using Core;
using Microsoft.Extensions.Logging;
using System;

namespace Otel.ConsoleApp
{
    internal class BatchService
    {
        private ILogger Logger { get => FnLoggerFactory.GetLog<BatchService>(); }

        public BatchService()
        {
        }

        internal void Call()
        {
            using (var parentActivity = DiagnosticsConfig.Source.StartActivity("ParentActivity"))
            {
                var rnd = new Random(Guid.NewGuid().GetHashCode());
                for (int i = 0; i < 5; i++)
                {
                    using (var childActivity = DiagnosticsConfig.Source.StartActivity("ChildActivity"))
                    {
                        var value = rnd.Next(100);
                        if (value % 2 == 0)
                        {
                            Logger.LogWarning($"Valore pari: {value}");
                        }
                        else
                        {
                            Logger.LogError($"Valore dispari: {value}");
                        }
                    }
                }
            }
        }
    }
}