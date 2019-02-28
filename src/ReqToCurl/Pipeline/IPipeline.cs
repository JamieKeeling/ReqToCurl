using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ReqToCurl.Pipeline
{
    public interface IPipeline
    {
        Task<string> ExecuteAsync(HttpContext context);
    }
}
