using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Shipping.NovaPoshta.Routing
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var lang = GetLanguageRoutePattern();
            
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.Shipping.NovaPoshta.SelectNpWarehouse",
                "Plugins/NovaPoshtaShipping/SelectNpWarehouse",
                new { controller = "NovaPoshtaHandle", action = "SelectWarehouse" });
            
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.Shipping.NovaPoshta.SaveCheckoutShippingAddress",
                "Plugins/NovaPoshtaShipping/SaveCheckoutShippingAddress",
                new { controller = "NovaPoshtaHandle", action = "SaveCheckoutShippingAddress" });
            
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.Shipping.NovaPoshta.UpdateDbNow",
                "Plugins/NovaPoshtaShipping/UpdateDbNow",
                new { controller = "NovaPoshtaHandle", action = "UpdateDbNow" });
            
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.Shipping.NovaPoshta.CheckProductsDimensionsAndWeight",
                "Plugins/NovaPoshtaShipping/CheckProductsDimensionsAndWeight",
                new { controller = "NovaPoshtaHandle", action = "CheckProductsDimensionsAndWeight" });

            endpointRouteBuilder.MapHub<NotifySignalRHub>("/npNotifyService");
        }

        public int Priority => 0;
        
        private string GetLanguageRoutePattern()
        {
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                var localizationSettings = EngineContext.Current.Resolve<LocalizationSettings>();
                if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    //this pattern is set once at the application start, when we don't have the selected language yet
                    //so we use 'en' by default for the language value, later it'll be replaced with the working language code
                    var code = "en";
                    return $"{{{NopPathRouteDefaults.LanguageRouteValue}:maxlength(2):{NopPathRouteDefaults.LanguageParameterTransformer}={code}}}";
                }
            }

            return string.Empty;
        }
    }
}