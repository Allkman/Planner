using Microsoft.Extensions.DependencyInjection;

namespace Planner.API.AutoMapper
{
    public static class AutoMapperServices
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {

            services.AddAutoMapper(
                typeof(ProjectMapperProfile),
                typeof(TicketMapperProfile)
  
                );
            return services;
        }
    }
}
