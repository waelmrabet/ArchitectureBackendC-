using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Strada.Framework.HealthCheck.AspNetCore
{
    /// <summary>
    /// HealthCheck extension
    /// </summary>
    public static class HealthCheckExtension
    {


        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        //[assembly: Microsoft.Extensions.DependencyInjection.Abstractions("5.0.0.0")]
        public static void ConfigureHealthCheck(this IServiceCollection services, IConfiguration configuration, string connexionString)
        {
            services.AddHealthChecks().AddNpgSql(configuration.GetConnectionString(connexionString), name: "DataBase Check");

        }

        public static void ConfigureHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthCheck", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json; charset=utf-8";
                    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(report));
                    await context.Response.Body.WriteAsync(bytes);
                }
            });
        }
    }
}



