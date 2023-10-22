using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

namespace Core
{
    public class FnMeterProvider
    {
        private static MeterProvider _meterProvider;

        public static void Initialize(string serviceName)
        {
            if (_meterProvider != null)
                throw new InvalidOperationException("FnMeterProvider already initialized!");

            _meterProvider = Sdk.CreateMeterProviderBuilder()
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: serviceName))
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri("http://localhost:4318/v1/metrics");
                    opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                })
                .Build();
        }

        public static void Dispose()
        {
            if (_meterProvider != null)
            {
                _meterProvider.ForceFlush();
                _meterProvider.Dispose();
            }
        }
    }
}
