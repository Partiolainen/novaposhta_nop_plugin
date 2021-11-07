using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Routing;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Plugin.Shipping.NovaPoshta.Services.Factories;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddScoped<ILocalizer, Localizer>();
            services.AddScoped<INovaPoshtaRepository<NovaPoshtaSettlement>, NovaPoshtaRepository<NovaPoshtaSettlement>>();
            services.AddScoped<INovaPoshtaRepository<NovaPoshtaWarehouse>, NovaPoshtaWarehousesRepository>();
            services.AddScoped<INovaPoshtaRepository<NovaPoshtaArea>, NovaPoshtaRepository<NovaPoshtaArea>>();
            services.AddScoped<INpScheduleTasksService, NpScheduleTasksService>();
            services.AddScoped<IRepository<Dimensions>, EntityRepository<Dimensions>>();
            services.AddScoped<INpApiService, NpApiService>();
            services.AddScoped<INpService, NpService>();
            services.AddScoped<INpOrderDataService, NpOrderDataService>();
            services.AddScoped<INpCustomerAddressService, NpCustomerAddressService>();
            services.AddScoped<IFactoriesService, FactoriesService>();
            services.AddScoped<INpProductService, NpProductService>();

            services.AddSingleton<INotificationServiceExt, NotificationServiceExt>();
            services.AddSignalR();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new NovaPoshtaViewLocationExpander());
            });
        }

        public int Order => 1;
    }
}