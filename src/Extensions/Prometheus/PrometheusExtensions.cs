using Strada.Framework.Extensions;
using Strada.Framework.Extensions.Prometheus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AppMetrics = Strada.Framework.Extensions.Prometheus.AppMetrics;
using ILoggerMs = Microsoft.Extensions.Logging.ILogger;

namespace Autofac
{
    /// <summary>
    /// PrometheusExtensions
    /// </summary>
    public static class PrometheusExtensions
    {
        /// <remarks>
        /// En debug l'écoute s'effectue par défaut en localhost,
        /// sinon sur toutes les interfaces réseaux disponibles.<br/>
        /// (port par défaut : 9187)
        /// </remarks>
        public static ContainerBuilder UsePrometheus(this ContainerBuilder builder, IConfiguration configuration, AutofacSetupAction setupAction = null)
        {
            IConfiguration section = configuration.GetSection("prometheus");

            int port = section.GetValue("port", AppMetrics.DEFAULT_PORT);
            if (port > 0)
            {
                //Problème de droit en debug si on exécute pas en tant qu'Admin avec VS
                string host = Run.IsDevelopment ? "localhost" : "+";

                string serverType = typeof(MetricServer).Name;
                string endpoint = $"{host}:{port}";

                try
                {
                    UseLoggerMetrics(builder);
                    UseAppMetrics(section);

                    setupAction?.Invoke(builder, configuration);

                    MetricServer server = new(host, port);
                    server.Start();

                    Run.HasMetrics = true;

                    Log.Debug("{metric_server} started on {metric_server_endpoint}", serverType, endpoint);
                }
                catch (Exception ex)
                {
                    //Ne pas bloquer toute l'application malgré tout...

                    Log.Error(ex, "Failed to start {metric_server} on {metric_server_endpoint}", serverType, endpoint);
                    Log.Debug("Try {metric_server_solution} instead of {metric_server_endpoint} for development", $"localhost:{port}", endpoint);
                }
            }

            return builder;
        }

        private static void UseLoggerMetrics(this ContainerBuilder builder)
        {
            builder.RegisterDecorator<LoggerDecorator, ILoggerMs>();
            builder.RegisterGenericDecorator(typeof(LoggerGenericDecorator<>), typeof(ILogger<>));
        }

        private static void UseAppMetrics(IConfiguration configuration)
        {
            string group = configuration.GetValue("group", "");

            string dateFormat = configuration.GetValue("date_format", "dd/MM/yyyy HH:mm:ss");
            DateTime bootDatetime = Process.GetCurrentProcess().StartTime.ToUniversalTime();

            string pod_hash = Run.IsInContainer ? GetPodHash(Environment.MachineName) : "";

            AssemblyName name = Assembly.GetEntryAssembly().GetName();

            IEnumerable<KeyValuePair<string, string>> labels = new Dictionary<string, string>()
            {
                ["name"] = name.Name,
                ["version"] = name.Version.ToString(),
                ["group"] = group,
                ["boot_datetime"] = bootDatetime.ToString(dateFormat),
                ["net_version"] = Environment.Version.ToString(),
                ["machine_name"] = Environment.MachineName,
                ["pod_hash"] = pod_hash,
                ["cpu_count"] = Environment.ProcessorCount.ToString(),
            };

            //Rajoute aussi toutes les variables d'environnement commençant par "METRIC_"
            //(ID de commit, branch...)
            IReadOnlyDictionary<string, string> customLabels = GetCustomLabels();
            if (customLabels.Any())
                labels = labels.Concat(customLabels);

            AppMetrics.AppInfoGauge = Metrics.CreateGauge("app_info_gauge", "Application/system's informations", labels.Select(x => x.Key).ToArray());
            AppMetrics.AppInfoGauge.WithLabels(labels.Select(x => x.Value).ToArray()).Set(1);

            AppMetrics.AppHealthCheckGauge.WithLabels("application").Set(1);

            string systemMemory = GetSystemMemory();
            if (long.TryParse(systemMemory, out long memory))
                AppMetrics.AppMemoryAvailableGauge.Set(memory);
        }

        /// <remarks>
        /// Linux uniquement
        /// </remarks>
        private static string GetSystemMemory()
        {
            string result;
            try
            {
                const string FILE = "/sys/fs/cgroup/memory/memory.limit_in_bytes";
                if (Run.IsInContainer && Environment.OSVersion.Platform == PlatformID.Unix && File.Exists(FILE))
                    result = File.ReadAllText(FILE).Trim(' ', '\r', '\n');
                else
                    result = "";
            }
            catch
            {
                result = "";
            }
            return result;
        }

        internal static string GetPodHash(string value)
        {
            Match match = Regex.Match(value, @"^(\w+-)+(?<pod_hash>\w{8,12}-\w{5})$");

            string result = match.Success ? match.Groups["pod_hash"].Value : "";
            return result;
        }

        private static IReadOnlyDictionary<string, string> GetCustomLabels() => GetCustomLabels(Environment.GetEnvironmentVariables());

        internal static IReadOnlyDictionary<string, string> GetCustomLabels(IDictionary variables)
        {
            Dictionary<string, string> result = new();

            foreach (DictionaryEntry entry in variables)
            {
                string key = entry.Key.ToString();
                if (key.StartsWith(AppMetrics.CUSTOM_LABEL_PREFIX))
                {
                    string value = entry.Value?.ToString();

                    if (!string.IsNullOrEmpty(value))
                    {
                        //Retire le préfixe
                        key = key[AppMetrics.CUSTOM_LABEL_PREFIX.Length..].ToLower();
                        result.TryAdd(key, value);
                    }
                }
            }

            return result;
        }
    }
}
