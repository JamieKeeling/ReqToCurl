using Microsoft.AspNetCore.Builder;

namespace ReqToCurl
{
    public static class RequestToCurlMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestToCurl(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<RequestToCurlMiddleware>();
        }
    }
}
