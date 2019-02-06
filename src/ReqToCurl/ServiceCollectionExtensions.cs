using Microsoft.Extensions.DependencyInjection;
using ReqToCurl.Pipeline;
using ReqToCurl.Steps;

namespace ReqToCurl
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReqToCurl(this IServiceCollection services)
        {
            services.AddTransient<ICurlExtractor, CurlExtractor>();
            services.AddTransient<IPipeline, Pipeline.Pipeline>();

            services.AddTransient<IExtractionStep, RequestHeaderStep>();

            return services;
        }
    }
}
