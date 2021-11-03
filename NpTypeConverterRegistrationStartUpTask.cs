using System.ComponentModel;
using System.Threading.Tasks;
using Nop.Core.Infrastructure;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.GenericAttributes;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.TypeConverters;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NpTypeConverterRegistrationStartUpTask : IStartupTask
    {
        public Task ExecuteAsync()
        {
            TypeDescriptor.AddAttributes(typeof(NpCustomerAddressForOrder), new TypeConverterAttribute(typeof(NpCustomerAddressTypeConverter)));
            TypeDescriptor.AddAttributes(typeof(ToWarehouseCustomerMainInfo), new TypeConverterAttribute(typeof(ToWarehouseCustomerMainInfoConverter)));
            
            return Task.CompletedTask;
        }

        public int Order => 2;
    }
}