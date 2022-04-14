using Strada.Framework.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System;
using System.Reflection;
using ILoggerMs = Microsoft.Extensions.Logging.ILogger;

namespace Autofac
{
    /// <summary>
    /// SerilogExtensions
    /// </summary>
    public static class SerilogExtensions
    {
        /// <remarks>
        /// En debug la sortie s'effectue par défaut sous une forme texte, <br/>
        /// sinon sous une forme d'un Json optimisée.
        /// </remarks>
        public static ContainerBuilder UseSerilog(this ContainerBuilder builder, IConfiguration configuration)
        {
            IConfiguration section = configuration.GetSection("serilog");

            LogEventLevel logLevel = section.GetValue("min_level", LogEventLevel.Information);

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .Enrich.FromLogContext();

            bool jsonOutput = section.GetValue("json_output", !Run.IsDevelopment);
            if (jsonOutput)
            {
                //Sortie sous la forme d'un Json
                loggerConfiguration.WriteTo.Console(new Serilog.Formatting.Compact.CustomJsonFormatter());
            }
            else
            {
                //Sortie sous une forme texte plus facile en lire en phase de DEV

                const string DEFAULT_TEMPLATE = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}";

                string outputTemplate = section.GetValue("text_output_template", DEFAULT_TEMPLATE);

                loggerConfiguration.WriteTo.Console(
                    theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Literate,
                    outputTemplate: outputTemplate);
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            AssemblyName name = Assembly.GetEntryAssembly().GetName();
            Log.Information("Start {application_name} - {application_version}", name.Name, name.Version);

            builder.Register(ctx =>
            {
                ILoggerProvider provider = new SerilogLoggerProvider();
                ILoggerFactory factory = new LoggerFactory(new ILoggerProvider[] { provider });

                ILoggerMs result = factory.CreateLogger("");
                return result;
            })
            .As<ILoggerMs>()
            .SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                   .As(typeof(ILogger<>))
                   .SingleInstance();

            return builder;
        }

        /// <summary>
        /// GetHttpLogEventLevel
        /// </summary>
        public static LogEventLevel GetHttpLogEventLevel(int statusCode, string path, Exception ex)
        {
            LogEventLevel result;

            if (statusCode >= 500 || ex != null)
            {
                result = LogEventLevel.Error;
            }
            else if (statusCode >= 400)
            {
                result = LogEventLevel.Warning;
            }
            else
            {
                //Abaisse le LogLevel des routes appellées régulièrement par les systèmes de surveillance + swagger

                const StringComparison COMPARISON = StringComparison.OrdinalIgnoreCase;

                if (path.EndsWith("/health", COMPARISON) ||
                    path.EndsWith("/metrics", COMPARISON) ||
                    path.StartsWith("/swagger", COMPARISON))
                    result = LogEventLevel.Verbose;
                else
                    result = LogEventLevel.Information;
            }

            return result;
        }
    }
}
