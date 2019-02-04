using Microsoft.AspNetCore.Http;
using ReqToCurl.Pipeline;
using System.Linq;

namespace ReqToCurl.Steps
{
    public class RequestHeaderStep : IExtractionStep
    {
        public bool CanExtract(HttpContext context)
        {
            return context.Request.Headers.Count > 0;
        }

        public string Extract(HttpContext context)
        {
            var requestHeaders = context.Request.Headers;

            return string.Join(" ", requestHeaders.Select(x => $"-H \"{x.Key} : {x.Value}\""));
        }
    }
}
