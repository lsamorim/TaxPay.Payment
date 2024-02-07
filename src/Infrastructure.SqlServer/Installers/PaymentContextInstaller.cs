using Infrastructure.SqlServer.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SqlServer.Installers
{
    public static class PaymentContextInstaller
    {
        public static IServiceCollection AddPaymentContext(this IServiceCollection services)
        {
            services.AddDbContext<PaymentContext>();
            return services;
        }
    }
}
