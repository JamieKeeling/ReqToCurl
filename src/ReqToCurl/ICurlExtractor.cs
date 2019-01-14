using Microsoft.AspNetCore.Http;

namespace ReqToCurl
{
    public interface ICurlExtractor
    {
        string ExtractRequest(HttpContext context);
    }
}
