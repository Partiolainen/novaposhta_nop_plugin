using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Shipping.NovaPoshta.Services;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddScoped<INovaPoshtaApiService, NovaPoshtaApiService>();
            services.AddScoped<INovaPoshtaService, NovaPoshtaService>();
        }

        public int Order => 1;
    }
}