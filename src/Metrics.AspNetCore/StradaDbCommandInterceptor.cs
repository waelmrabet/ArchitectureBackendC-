using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;
using Prometheus;
using DbCommandInterceptor = Microsoft.EntityFrameworkCore.Diagnostics.DbCommandInterceptor;

namespace Strada.Time.Api.Extensions
{
    /// <summary>
    /// Authentication extension
    /// </summary>
    public class StradaDbCommandInterceptor : DbCommandInterceptor
    {
        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>

        public Histogram ResponseTimeHistogram { get; set; }

        public StradaDbCommandInterceptor() : base()
        {
            // Custom Metrics to count requests for each endpoint and the method
            ResponseTimeHistogram = Metrics.CreateHistogram("db_query_duration_seconds",
                                                                        "La durée de traitement d'une requête",
                                                                        new HistogramConfiguration
                                                                        {
                                                                            Buckets = Histogram.ExponentialBuckets(0.01, 2, 1000),
                                                                            LabelNames = new[] { "Table", "Action" }
                                                                        });

        }

        public override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            try
            {
                //
                if (command.CommandText.ToUpper().StartsWith("S"))
                {
                    ResponseTimeHistogram.Labels(command.CommandText.ToUpper().Split("FROM")[1].Split("AS")[0].ToLower().Replace("\"", ""), "Select").Observe(eventData.Duration.TotalMilliseconds);
                }
                else
                {
                    if (command.CommandText.ToUpper().StartsWith("I"))
                    {
                        ResponseTimeHistogram.Labels(command.CommandText.ToUpper().Remove(0, 12).Split(" ")[0].ToLower().Replace("\"", ""), "Insert").Observe(eventData.Duration.TotalMilliseconds);
                    }
                    else if (command.CommandText.ToUpper().StartsWith("U"))
                    {
                        ResponseTimeHistogram.Labels(command.CommandText.ToUpper().Remove(0, 7).Split(" ")[0].ToLower().Replace("\"", ""), "Update").Observe(eventData.Duration.TotalMilliseconds);
                    }
                }

                ResponseTimeHistogram.Labels(command.CommandText.ToUpper().Split("FROM")[1].Split("AS")[0]).Observe(eventData.Duration.TotalMilliseconds);
                return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);

            }
            catch
            {
                return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
            }
        }
    }
}
