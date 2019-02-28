using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ReqToCurl.Steps
{
    public interface IExtractionStep
    {
        bool CanExtract(HttpContext context);
        Task<string> ExtractAsync(HttpContext context);
    }
}
