using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace Strada.Framework.Metrics.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class PrometheusExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMetrics(this IServiceCollection services)
        {
            services.AddControllers(options =>
                    {
                        options.Filters.Add(new MetricErrorHandlingFilter());
                    });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureMetrics(this IApplicationBuilder app)
        {
            app.UseMiddleware<CaptureRouteDataMiddleware>();
            app.UseMiddleware<HttpRequestCountMiddleware>(new HttpRequestCountOptions());
            
            //Enable middleware to serve swagger - ui(HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
            });

        }
    }
}
