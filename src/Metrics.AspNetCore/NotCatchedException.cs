using Microsoft.AspNetCore.Http;
using Prometheus;

namespace Strada.Framework.Metrics.AspNetCore
{

    internal sealed class NotCatchedException : HttpRequestMiddlewareBase<ICollector<ICounter>, ICounter>
    {

        public NotCatchedException(HttpRequestCountOptions options)
            : base(options, options?.Counter)
        {

        }

        public void CreateMetricInstance(HttpContext context)
        {
            CreateChild(context).Inc();
        }

        protected override string[] DefaultLabels => HttpRequestLabelNames.All;

        protected override ICollector<ICounter> CreateMetricInstance(string[] labelNames) => MetricFactory.CreateCounter(
            "Exceptions", "Total des exceptions non catchés.", new CounterConfiguration
                                                                {
                                                                    LabelNames = labelNames
                                                                }
            );
    }
}
