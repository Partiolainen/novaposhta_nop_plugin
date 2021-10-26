using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NovaPoshtaViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.ControllerName == "NpPluginOrders")
            {
                var result = new List<string>(viewLocations)
                {
                    "/Plugins/Shipping.NovaPoshta/Views/{1}/{0}" + RazorViewEngine.ViewExtension,
                    "/Plugins/Shipping.NovaPoshta/Areas/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension
                };

                return result;
            }

            return viewLocations;
        }
    }
}