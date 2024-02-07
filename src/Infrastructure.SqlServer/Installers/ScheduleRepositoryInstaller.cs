using Domain.ScheduleAggregate;
using Infrastructure.SqlServer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SqlServer.Installers
{
    public static class ScheduleRepositoryInstaller
    {
        public static IServiceCollection AddScheduleRepository(this IServiceCollection services)
        {
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            return services;
        }
    }
}
