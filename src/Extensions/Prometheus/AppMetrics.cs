using Prometheus;
using Factory = Prometheus.Metrics;

namespace Strada.Framework.Extensions.Prometheus
{
    /// <summary>
    /// AppMetrics
    /// </summary>
    public static class AppMetrics
    {
        /// <summary>
        /// DEFAULT_PORT
        /// </summary>
        public const int DEFAULT_PORT = 9187;

        /// <summary>
        /// CUSTOM_LABEL_PREFIX
        /// </summary>
        public const string CUSTOM_LABEL_PREFIX = "METRIC_";

        /// <summary>
        /// AppInfoGauge
        /// </summary>
        public static Gauge AppInfoGauge { get; internal set; }

        /// <summary>
        /// AppLogsCounter
        /// </summary>
        public static Counter AppLogsCounter { get; } =
            Factory.CreateCounter("app_logs_total",
                                  "Total number of warning/error/critical logs",
                                  "log_level");

        /// <summary>
        /// AppMemoryAvailableGauge
        /// </summary>
        public static Gauge AppMemoryAvailableGauge { get; } =
            Factory.CreateGauge("app_total_memory_available_bytes",
                                "Total available memory");

        /// <summary>
        /// AppHealthCheckGauge
        /// </summary>
        public static Gauge AppHealthCheckGauge { get; } =
            Factory.CreateGauge("app_health_check_gauge",
                                "Health check status",
                                "name");
    }
}
