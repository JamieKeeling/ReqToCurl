using Microsoft.AspNetCore.Http;

namespace ReqToCurl.Pipeline
{
    interface IPipeline
    {
        string Execute(HttpContext context);
    }
}
