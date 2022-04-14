using Microsoft.Extensions.Logging;

namespace Strada.Framework.Extensions.Prometheus
{
    /// <summary>
    /// LoggerGenericDecorator
    /// </summary>
    public class LoggerGenericDecorator<TCategoryName> : LoggerDecorator, ILogger<TCategoryName>
    {
        /// <summary>
        /// LoggerGenericDecorator
        /// </summary>
        public LoggerGenericDecorator(ILogger logger) : base(logger) { }
    }
}
