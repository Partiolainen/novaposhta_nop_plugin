using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NpCustomerAddressTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is not string valueStr) 
                return base.ConvertFrom(context, culture, value);

            return string.IsNullOrEmpty(valueStr) ? null : JsonSerializer.Deserialize<NpCustomerAddressForOrder>(valueStr);
        }
        
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) 
                return base.ConvertTo(context, culture, value, destinationType);

            return value is not NpCustomerAddressForOrder ? string.Empty : JsonSerializer.Serialize(value);
        }
    }
}