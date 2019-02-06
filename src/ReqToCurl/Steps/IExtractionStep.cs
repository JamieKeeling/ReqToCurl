using Microsoft.AspNetCore.Http;

namespace ReqToCurl.Steps
{
    public interface IExtractionStep
    {
        bool CanExtract(HttpContext context);
        string Extract(HttpContext context);
    }
}
