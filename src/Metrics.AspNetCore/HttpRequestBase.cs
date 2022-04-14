using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Prometheus;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Strada.Framework.Metrics.AspNetCore
{
    public abstract class HttpRequestMiddlewareBase<TCollector, TChild>
        where TCollector : class, ICollector<TChild>
        where TChild : class, ICollectorChild
    {

        protected abstract string[] DefaultLabels { get; }
        protected abstract TCollector CreateMetricInstance(string[] labelNames);
        protected MetricFactory MetricFactory { get; }

        private readonly ICollection<HttpRouteParameterMapping> _additionalRouteParameters;
        private readonly TCollector _metric;

        private readonly Dictionary<string, string> _labelToRouteParameterMap;

        private readonly bool _labelsRequireRouteData;

        protected HttpRequestMiddlewareBase(HttpMetricsOptionsBase options, TCollector customMetric)
        {
            MetricFactory = Prometheus.Metrics.WithCustomRegistry(options?.Registry ?? Prometheus.Metrics.DefaultRegistry);

            _additionalRouteParameters = options?.AdditionalRouteParameters ?? new List<HttpRouteParameterMapping>(0);

            //ValidateAdditionalRouteParameterSet();
            _labelToRouteParameterMap = CreateLabelToRouteParameterMap();

            if (customMetric != null)
            {
                _metric = customMetric;
            }
            else
            {
                _metric = CreateMetricInstance(CreateDefaultLabelSet());
            }

            _labelsRequireRouteData = _metric.LabelNames.Except(HttpRequestLabelNames.NonRouteSpecific).Any();
        }

        /// <summary>
        /// Creates the metric child instance to use for measurements.
        /// </summary>
        /// <remarks>
        /// Internal for testing purposes.
        /// </remarks>
        protected internal TChild CreateChild(HttpContext context)
        {
            if (!_metric.LabelNames.Any())
                return _metric.Unlabelled;

            if (!_labelsRequireRouteData)
                return CreateChild(context, null);

            var routeData = context.Features.Get<ICapturedRouteDataFeature>()?.Values;

            if (routeData == null)
                routeData = context.GetRouteData()?.Values;

            return CreateChild(context, routeData);
        }

        protected TChild CreateChild(HttpContext context, RouteValueDictionary routeData)
        {
            var labelValues = new string[_metric.LabelNames.Length];

            for (var i = 0; i < labelValues.Length; i++)
            {
                switch (_metric.LabelNames[i])
                {
                    case HttpRequestLabelNames.Method:
                        labelValues[i] = context.Request.Method;
                        break;
                    case HttpRequestLabelNames.Code:
                        labelValues[i] = context.Response.StatusCode.ToString(CultureInfo.InvariantCulture);
                        break;
                    default:
                        // Validation du set du label 
                        var parameterName = _labelToRouteParameterMap[_metric.LabelNames[i]];
                        labelValues[i] = routeData?[parameterName] as string ?? string.Empty;
                        break;
                }
            }

            return _metric.WithLabels(labelValues);
        }

        private string[] CreateDefaultLabelSet()
        {
            return DefaultLabels.Concat(_additionalRouteParameters.Select(x => x.LabelName)).ToArray();
        }

        private Dictionary<string, string> CreateLabelToRouteParameterMap()
        {
            var map = new Dictionary<string, string>(_additionalRouteParameters.Count + 2);
            map["action"] = "action";
            map["controller"] = "controller";

            // additionnel parametre de routage
            foreach (var entry in _additionalRouteParameters)
                map[entry.LabelName] = entry.ParameterName;

            return map;
        }
    }

    public abstract class HttpMetricsOptionsBase
    {
        // Un paramètre a jouter en plus de controleur et action pour gérer par exemple la version de l'api
        public List<HttpRouteParameterMapping> AdditionalRouteParameters { get; set; } = new List<HttpRouteParameterMapping>();

        public CollectorRegistry Registry { get; set; }
    }
}
