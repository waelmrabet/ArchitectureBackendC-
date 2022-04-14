using Microsoft.AspNetCore.Http;
using Prometheus;
using System.Threading.Tasks;

namespace Strada.Framework.Metrics.AspNetCore
{

    internal sealed class HttpRequestCountMiddleware : HttpRequestMiddlewareBase<ICollector<ICounter>, ICounter>
    {
        private readonly RequestDelegate _next;

        public HttpRequestCountMiddleware(RequestDelegate next, HttpRequestCountOptions options)
            : base(options, options?.Counter)
        {
            _next = next; 
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                CreateChild(context).Inc();
            }
        }

        protected override string[] DefaultLabels => HttpRequestLabelNames.All;

        protected override ICollector<ICounter> CreateMetricInstance(string[] labelNames) => MetricFactory.CreateCounter(
                "Http_requetes_count",
                "Strada Framework - Total des requêtes HTTP traitées.",
                new CounterConfiguration
                {
                    LabelNames = labelNames
                });
    }

    public sealed class HttpRequestCountOptions : HttpMetricsOptionsBase
    {
        public ICollector<ICounter> Counter { get; set; }
    }
}
