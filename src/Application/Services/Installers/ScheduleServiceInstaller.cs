using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Installers
{
    public static class ScheduleServiceInstaller
    {
        public static IServiceCollection AddScheduleService(this IServiceCollection services)
        {
            services.AddScoped<IScheduleService, ScheduleService>();
            return services;
        }
    }
}
