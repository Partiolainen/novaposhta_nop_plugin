using System.ComponentModel;
using System.Threading.Tasks;
using Nop.Core.Infrastructure;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NpTypeConverter : IStartupTask
    {
        public Task ExecuteAsync()
        {
            // TypeDescriptor.AddAttributes(typeof(NpCustomerAddressForOrder),
            //     new TypeConverterAttribute(typeof(NpCustomerAddressTypeConverter)));
            
            return Task.CompletedTask;
        }

        public int Order => 2;
    }
}