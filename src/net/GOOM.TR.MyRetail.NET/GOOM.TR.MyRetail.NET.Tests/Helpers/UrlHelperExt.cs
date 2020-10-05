using System;
using System.Globalization;
using System.Linq;

namespace GOOM.TR.MyRetail.NET.Tests.Helpers
{
    public static class UrlHelperExt
    {
        public static string ToUrlParameter(this DateTime? dateTime)
        {
            return dateTime == null
                ? null
                : dateTime.Value.ToUniversalTime().ToString("s", CultureInfo.InvariantCulture);
        }

        public static string ToUrlParameter(this DateTimeOffset? dateTimeOffset)
        {
            return dateTimeOffset == null
                ? null
                : dateTimeOffset.Value.UtcDateTime.ToString("s", CultureInfo.InvariantCulture);
        }

        public static string BuildUrlPath(params object[] pathSegmentObjects)
        {
            if (pathSegmentObjects == null) return null;
            var pathSegnments = pathSegmentObjects.Select(ps => ps?.ToString())
                .ToArray();

            return BuildUrlPath(pathSegments: pathSegnments);
        }

        public static string BuildUrlPath(params string[] pathSegments)
        {
            if (pathSegments == null) return null;
            if (pathSegments.Any(ps => string.IsNullOrWhiteSpace(ps)))
                throw new InvalidOperationException($"Path segment cannot be null: {pathSegments.ToJson()}");

            return string.Join("/", pathSegments).Replace("//", "/");
        }

        public static Uri Append(this Uri baseUrl, params string[] pathSegments)
        {
            var relativeUrl = BuildUrlPath(pathSegments);
            return new Uri($"{baseUrl.AbsoluteUri.TrimEnd('/')}/{relativeUrl.TrimStart('/')}");
        }
    }
}
