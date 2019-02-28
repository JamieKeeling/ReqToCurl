using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ReqToCurl
{
    public interface ICurlExtractor
    {
        Task<string> ExtractRequestAsync(HttpContext context);
    }
}
