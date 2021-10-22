using System;
using System.Collections.Generic;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.NovaPoshta.Models
{
    public class NovaPoshtaConfigurationSettingsModel
    {
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.ApiKey")]
        public string ApiKey { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.ApiUrl")]
        public string ApiUrl { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.UseAdditionalFee")]
        public bool UseAdditionalFee { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.AdditionalFeeIsPercent")]
        public bool AdditionalFeeIsPercent { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.AdditionalFee")]
        public float AdditionalFee { get; set; }

        public bool DbUpdateStarted { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.DbLastSuccessUpdate")]
        public DateTime? DbLastSuccessUpdate { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.WarehouseCities")]
        public List<WarehouseAvailability> WarehouseCities { get; set; }

        public record WarehouseAvailability
        {
            public Warehouse Warehouse { get; set; }
            public Address Address { get; set; }
            public bool IsAvailable { get; set; }
            public int NovaPoshtaWarehousesCount { get; set; }
        }
    }
}