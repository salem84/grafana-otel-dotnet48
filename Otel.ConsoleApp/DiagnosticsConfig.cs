using System.Diagnostics;

namespace Otel.ConsoleApp
{
    internal static class DiagnosticsConfig
    {
        public const string SourceName = "Fn.Batch.SampleApp";
        public static readonly ActivitySource Source = new ActivitySource(SourceName);

    }
}
