using Microsoft.Extensions.Logging;
using Strada.Framework.Extensions.Prometheus;
using System;

namespace Strada.Framework.Extensions.Prometheus
{
    /// <summary>
    /// Ajoute un compteur sur les logs signalant un problème : Warning, Error ou Critical
    /// </summary>
    public class LoggerDecorator : ILogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// LoggerDecorator
        /// </summary>
        public LoggerDecorator(
            ILogger logger)
        {
            _logger = logger;
            InitializeCounters();            
        }

        private static void InitializeCounters()
        {
            foreach (LogLevel logLevel in new LogLevel[] { LogLevel.Warning, LogLevel.Error, LogLevel.Critical })
            {
                string label = GetLabel(logLevel);
                AppMetrics.AppLogsCounter.WithLabels(label).Publish();
            }
        }

        private static string GetLabel(LogLevel logLevel) => logLevel.ToString();

        /// <summary>
        /// BeginScope
        /// </summary>
        public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);

        /// <summary>
        /// IsEnabled
        /// </summary>
        public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

        /// <summary>
        /// Log
        /// </summary>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);

            if (logLevel == LogLevel.Warning ||
                logLevel == LogLevel.Error ||
                logLevel == LogLevel.Critical)
            {
                string label = GetLabel(logLevel);
                AppMetrics.AppLogsCounter.WithLabels(label).Inc();
            }
        }
    }
}
