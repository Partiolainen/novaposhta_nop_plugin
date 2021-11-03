using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public static class LocalizationConst
    {
        public static string ShippingMethodName => "Plugins.Shipping.NovaPoshta.views.shippingMethodName";
        public static string ShippingMethodToWarehouse => "Plugins.Shipping.NovaPoshta.views.shippingMethodToWarehouse";
        public static string ShippingMethodAddress => "Plugins.Shipping.NovaPoshta.views.shippingMethodAddress";
        public static string ApiKey => "Plugins.Shipping.NovaPoshta.Fields.ApiKey";
        public static string ApiUrl => "Plugins.Shipping.NovaPoshta.Fields.ApiUrl";
        public static string UseAdditionalFee => "Plugins.Shipping.NovaPoshta.Fields.UseAdditionalFee";
        public static string AdditionalFeeIsPercent => "Plugins.Shipping.NovaPoshta.Fields.AdditionalFeeIsPercent";
        public static string AdditionalFee => "Plugins.Shipping.NovaPoshta.Fields.AdditionalFee";
        public static string DbLastSuccessUpdate => "Plugins.Shipping.NovaPoshta.Fields.DbLastSuccessUpdate";
        public static string WarehouseCities => "Plugins.Shipping.NovaPoshta.Fields.WarehouseCities";
        public static string WarehouseUnavailableMessage => "Plugins.Shipping.NovaPoshta.Fields.WarehouseUnavailableMessge";
        public static string ShippingDetails => "Plugins.Shipping.NovaPoshta.Fields.ShippingDetails";
        public static string ShippingDetailsToWarehouse => "Plugins.Shipping.NovaPoshta.Fields.ShippingPointDetails";
        public static string ShippingDetailsToAddress => "Plugins.Shipping.NovaPoshta.Fields.ShippingAddressDetails";
        public static string ShippingPointNumber => "Plugins.Shipping.NovaPoshta.Fields.ShippingPointNumber";
        public static string ShippingPoint => "Plugins.Shipping.NovaPoshta.Fields.ShippingPoint";
        public static string ChangeShippingPoint => "Plugins.Shipping.NovaPoshta.Fields.ChangeShippingPoint";
        public static string CreateShipmentWaybill => "Plugins.Shipping.NovaPoshta.Fields.CreateShipmentWaybill";
        public static string ShippingAddressTitle => "Plugins.Shipping.NovaPoshta.Fields.ShippingAddressTitle";
        public static string RecipientMainDataTitle => "Plugins.Shipping.NovaPoshta.Fields.RecipientMainDataTitle";
        public static string ChangeData => "Plugins.Shipping.NovaPoshta.Fields.ChangeData";
        public static string SelectBranch => "Plugins.Shipping.NovaPoshta.Fields.SelectBranch";

        #region Address

        public static string ZipPostalCode => "Plugins.Shipping.NovaPoshta.Address.ZipPostalCode";
        public static string Area => "Plugins.Shipping.NovaPoshta.Address.Area";
        public static string Region => "Plugins.Shipping.NovaPoshta.Address.Region";
        public static string City => "Plugins.Shipping.NovaPoshta.Address.City";
        public static string Street => "Plugins.Shipping.NovaPoshta.Address.Street";
        public static string House => "Plugins.Shipping.NovaPoshta.Address.House";
        public static string Flat => "Plugins.Shipping.NovaPoshta.Address.Flat";
        public static string PhoneNumber => "Plugins.Shipping.NovaPoshta.Address.PhoneNumber";
        public static string FirstName => "Plugins.Shipping.NovaPoshta.Address.FirstName";
        public static string LastName => "Plugins.Shipping.NovaPoshta.Address.LastName";

        #endregion

        public static IList<string> GetValues()
        {
            return typeof(LocalizationConst).GetMethods().Select(methodInfo => methodInfo.Name).ToList();
        }
    }
}