using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using ReqToCurl.Pipeline;
using System.Text;
using System.Threading.Tasks;

namespace ReqToCurl
{
    public class CurlExtractor : ICurlExtractor
    {
        private readonly StringBuilder _curlRequest = new StringBuilder();
        private readonly IPipeline _extractionPipeline;

        public CurlExtractor(IPipeline extractionPipeline)
        {
            _extractionPipeline = extractionPipeline;
        }

        public async Task<string> ExtractRequestAsync(HttpContext context)
        {
            _curlRequest.AppendLine($"curl {context.Request.GetEncodedUrl()}");

            var extractedData = await _extractionPipeline.ExecuteAsync(context).ConfigureAwait(false);

            if(!string.IsNullOrWhiteSpace(extractedData))
                _curlRequest.AppendLine(extractedData);

            return _curlRequest.ToString();
        }
    }
}
