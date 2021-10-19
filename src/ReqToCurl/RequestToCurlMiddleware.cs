using Microsoft.AspNetCore.Http;
using ReqToCurl.Logger;
using System.Threading.Tasks;

namespace ReqToCurl
{
    public class RequestToCurlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISimpleLogger<RequestToCurlMiddleware> _logger;
        private readonly ICurlExtractor _curlExtractor;

        public RequestToCurlMiddleware(RequestDelegate next, ISimpleLogger<RequestToCurlMiddleware> logger, ICurlExtractor curlExtractor)
        {
            _next = next;
            _logger = logger;
            _curlExtractor = curlExtractor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline
            await _next(context);

            if (context.Response.HasStarted)
            {
                _logger.Information($"Response has started with a HTTP {context.Response.StatusCode} value");
                _logger.Information(await _curlExtractor.ExtractRequestAsync(context).ConfigureAwait(false));
            }
        }
    }
}
