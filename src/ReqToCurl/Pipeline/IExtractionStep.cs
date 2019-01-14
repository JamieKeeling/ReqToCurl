using Microsoft.AspNetCore.Http;

namespace ReqToCurl.Pipeline
{
    public interface IExtractionStep
    {
        bool CanExtract(HttpContext context);
        bool Extract(HttpContext context);
    }
}
