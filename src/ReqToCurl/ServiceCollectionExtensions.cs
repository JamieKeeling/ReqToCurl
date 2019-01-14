using Microsoft.Extensions.DependencyInjection;

namespace ReqToCurl
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReqToCurl(this IServiceCollection services)
        {
            services.AddTransient<ICurlExtractor, CurlExtractor>();

            return services;
        }
    }
}
