using Microsoft.Extensions.DependencyInjection;

namespace ReqToCurl
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReqToCurl(this IServiceCollection services)
        {
            //TODO: Configure DI here
            return services;
        }
    }
}
