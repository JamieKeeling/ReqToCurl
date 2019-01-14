using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ReqToCurl
{
    public class RequestToCurlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestToCurlMiddleware> _logger;
        private readonly ICurlExtractor _curlExtractor;

        public RequestToCurlMiddleware(RequestDelegate next, ILogger<RequestToCurlMiddleware> logger, ICurlExtractor curlExtractor)
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
                //TODO: Verify response status code matches options, otherwise ignore
                _logger.Log(LogLevel.Information, $"Response has started with a HTTP {context.Response.StatusCode} value");
                _logger.Log(LogLevel.Information, _curlExtractor.ExtractRequest(context));
            }
        }
    }
}   
