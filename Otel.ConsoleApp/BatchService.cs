using Core;
using Microsoft.Extensions.Logging;

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
            Logger.LogWarning("missing params");
        }
    }
}