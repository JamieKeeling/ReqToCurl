using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace ReqToCurl.Steps
{
    public class RequestHeaderStep : IExtractionStep
    {
        public bool CanExtract(HttpContext context)
        {
            return context.Request.Headers.Count > 0;
        }

        public async Task<string> ExtractAsync(HttpContext context)
        {
            var requestHeaders = context.Request.Headers;

            return await Task.FromResult(string.Join(" ", requestHeaders.Select(x => $"-H \"{x.Key} : {x.Value}\"")));
        }
    }
}
