using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using ReqToCurl.Pipeline;
using System.Text;
using System.Threading.Tasks;

namespace ReqToCurl
{
    public class CurlExtractor : ICurlExtractor
    {
        private StringBuilder curlRequest = new StringBuilder();
        private readonly IPipeline _extractionPipeline;

        public CurlExtractor(IPipeline extractionPipeline)
        {
            _extractionPipeline = extractionPipeline;
        }

        public async Task<string> ExtractRequestAsync(HttpContext context)
        {
            curlRequest.AppendLine($"curl {context.Request.GetEncodedUrl()}");

            var extractedData = await _extractionPipeline.ExecuteAsync(context);

            if(!string.IsNullOrWhiteSpace(extractedData))
                curlRequest.AppendLine(await _extractionPipeline.ExecuteAsync(context));

            return curlRequest.ToString();
        }
    }
}
