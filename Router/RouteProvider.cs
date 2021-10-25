using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Shipping.NovaPoshta.Router
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.Shipping.NovaPoshta.SelectNpWarehouse",
                "Plugins/NovaPoshtaShipping/SelectNpWarehouse",
                new { controller = "NovaPoshtaHandle", action = "SelectWarehouse" });
        }

        public int Priority => 0;
    }
}