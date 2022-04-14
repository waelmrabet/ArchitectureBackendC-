using Microsoft.AspNetCore.Mvc.Filters;

namespace Strada.Framework.Metrics.AspNetCore
{
     /// <summary>
     /// Dans le cas ou nous installons les metrics sur un projet sans exception middleware
     /// </summary>
    public class MetricErrorHandlingFilter : ExceptionFilterAttribute
    {
      /// <summary>
      /// pour lancer la création du metric exception counter
      /// </summary>
      /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var metric = new NotCatchedException(new HttpRequestCountOptions());
            metric.CreateMetricInstance(context.HttpContext);
            context.ExceptionHandled = false;
        }      
    }
}
