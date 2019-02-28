using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ReqToCurl.Pipeline
{
    interface IPipeline
    {
        Task<string> ExecuteAsync(HttpContext context);
    }
}
