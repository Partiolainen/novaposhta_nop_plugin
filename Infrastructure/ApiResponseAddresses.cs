using System.Collections.Generic;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure
{
    public class ApiResponseAddresses
    {
        public int TotalCount { get; set; }
        public List<NovaPoshtaAddress> Addresses { get; set; }
    }
}