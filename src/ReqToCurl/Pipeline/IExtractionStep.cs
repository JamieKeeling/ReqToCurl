using Microsoft.AspNetCore.Http;

namespace ReqToCurl.Pipeline
{
    public interface IExtractionStep
    {
        bool CanExtract(HttpContext context);
        string Extract(HttpContext context);
    }
}
