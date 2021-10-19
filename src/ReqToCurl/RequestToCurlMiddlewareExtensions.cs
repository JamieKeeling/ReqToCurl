using Microsoft.AspNetCore.Builder;

namespace ReqToCurl
{
    public static class RequestToCurlMiddlewareExtensions
    {
        public static IApplicationBuilder UseReqToCurl(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<RequestToCurlMiddleware>();
        }
    }
}
