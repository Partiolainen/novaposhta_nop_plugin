using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.GenericAttributes;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.TypeConverters
{
    public class ToWarehouseCustomerMainInfoConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is not string valueStr) 
                return base.ConvertFrom(context, culture, value);

            return string.IsNullOrEmpty(valueStr) ? null : JsonSerializer.Deserialize<ToWarehouseCustomerMainInfo>(valueStr);
        }
        
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) 
                return base.ConvertTo(context, culture, value, destinationType);

            return value is not ToWarehouseCustomerMainInfo ? string.Empty : JsonSerializer.Serialize(value);
        }
    }
}