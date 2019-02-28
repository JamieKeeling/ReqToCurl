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
            services.AddTransient<IPipeline, ExtractionPipeline>();

            services.AddTransient<IExtractionStep, RequestHeaderStep>();
            services.AddTransient<IExtractionStep, RequestDataStep>();

            return services;
        }
    }
}
