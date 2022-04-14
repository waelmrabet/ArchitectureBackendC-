
namespace Strada.Framework.Metrics.AspNetCore
{
    public static class HttpRequestLabelNames
    {
        public const string Code = "code";
        public const string Method = "method";
        public const string Controller = "controller";
        public const string Action = "action";

        public static readonly string[] All =
        {
            Code,
            Method,
            Controller,
            Action
        };

        internal static readonly string[] PotentiallyAvailableBeforeExecutingFinalHandler =
        {
            Method,
            Controller,
            Action
        };

        // Labels qui n'ont pas besoin d'informations de routage pour etre collectées
        internal static readonly string[] NonRouteSpecific =
        {
            Code,
            Method
        };
    }
}
