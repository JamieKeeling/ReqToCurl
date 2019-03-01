using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using System.Threading.Tasks;

namespace ReqToCurl.Steps
{
    public class RequestDataStep : IExtractionStep
    {
        public bool CanExtract(HttpContext context)
        {
            return context.Request.Body.Length > 0;
        }

        public async Task<string> ExtractAsync(HttpContext context)
        {
            context.Request.EnableRewind();

            using (var reader = new StreamReader(context.Request.Body))
            {
                var requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);

                //Reset to the beginning so subsequent reads see the content
                context.Request.Body.Seek(0, SeekOrigin.Begin);

                return $"-d '{requestBody}'";
            }
        }
    }
}
