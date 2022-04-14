using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Strada.Framework.Metrics.AspNetCore
{

    interface ICapturedRouteDataFeature
    {
        RouteValueDictionary Values { get; }
    }


    sealed class CapturedRouteDataFeature : ICapturedRouteDataFeature
    {
        public RouteValueDictionary Values { get; } = new RouteValueDictionary();
    }

    internal sealed class CaptureRouteDataMiddleware
    {
        private readonly RequestDelegate _next;

        public CaptureRouteDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            TryCaptureRouteData(context);

            return _next(context);
        }

        private static void TryCaptureRouteData(HttpContext context)
        {
            var routeData = context.GetRouteData();

            if (routeData == null || routeData.Values.Count <= 0)
                return;

            var capturedRouteData = new CapturedRouteDataFeature();

            foreach (var pair in routeData.Values)
                capturedRouteData.Values.Add(pair.Key, pair.Value);

            context.Features.Set<ICapturedRouteDataFeature>(capturedRouteData);
        }
    }
}
