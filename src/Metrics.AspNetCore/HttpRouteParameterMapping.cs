namespace Strada.Framework.Metrics.AspNetCore
{
    public sealed class HttpRouteParameterMapping
    {
        /// <summary>
        /// Name of the HTTP route parameter.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Name of the Prometheus label.
        /// </summary>
        public string LabelName { get; }

        public HttpRouteParameterMapping(string name)
        {
            //Collector.ValidateLabelName(name);

            ParameterName = name;
            LabelName = name;
        }

        public HttpRouteParameterMapping(string parameterName, string labelName)
        {
            //Collector.ValidateLabelName(labelName);

            ParameterName = parameterName;
            LabelName = labelName;
        }

        public static implicit operator HttpRouteParameterMapping(string name) => new HttpRouteParameterMapping(name);
    }
}
