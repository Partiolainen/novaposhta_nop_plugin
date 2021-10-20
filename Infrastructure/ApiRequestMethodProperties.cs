using System;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure
{
    public class ApiRequestMethodProperties
    {
        public string Ref { get; set; }
        public string CityRef { get; set; }
        public string CityName { get; set; }

        public ApiRequestMethodProperties(string @ref = null, string cityRef = null, string cityName = null)
        {
            Ref = @ref;
            CityRef = cityRef;
            CityName = cityName;
        }
    }
}