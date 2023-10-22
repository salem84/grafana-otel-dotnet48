using OpenTelemetry;
using OpenTelemetry.Trace;
using System;

namespace Core
{
    public class FnTraceProvider
    {
        private static TracerProvider _tracerProvider;

        public static void Initialize()
        {
            if (_tracerProvider != null)
                throw new InvalidOperationException("FnTraceProvider already initialized!");

            _tracerProvider = Sdk.CreateTracerProviderBuilder()
            //.AddAspNetInstrumentation()

            // Other configuration, like adding an exporter and setting resources
            .AddSource("*")
            .AddConsoleExporter()
            .AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri("http://localhost:4318/v1/traces");
                opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            })
            //.SetResourceBuilder(
            //    ResourceBuilder.CreateDefault()
            //        .AddService(serviceName: "my-service-name", serviceVersion: "1.0.0"))

            .Build();
        }

        public static void Dispose()
        {
            if (_tracerProvider != null)
            {
                _tracerProvider.Dispose();
            }
        }
    }
}
