using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using ReqToCurl.Pipeline;
using System.Collections.Generic;
using System.Text;

namespace ReqToCurl
{
    public class CurlExtractor : ICurlExtractor
    {
        private StringBuilder curlRequest = new StringBuilder();
        private readonly IEnumerable<IExtractionStep> _extractionSteps; 

        public CurlExtractor(IEnumerable<IExtractionStep> extractionSteps)
        {
            _extractionSteps = extractionSteps;
        }

        public string ExtractRequest(HttpContext context)
        {
            curlRequest.AppendLine($"curl {context.Request.GetEncodedUrl()}");
            
            return curlRequest.ToString();
        }
    }
}
