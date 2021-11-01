using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest;
using Nop.Services.Configuration;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NpApiService : INpApiService
    {
        private readonly ISettingService _settingService;

        public NpApiService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<List<NovaPoshtaAddress>> GetAddressesByCityName(string cityName)
        {
            var apiResponse = await NovaPoshtaApiGetData<ApiResponseAddresses, GetAddressesProps>(props =>
            {
                props.ModelName = "Address";
                props.CalledMethod = "searchSettlements";
                props.CityName = cityName;
            });

            var addresses = new List<NovaPoshtaAddress>();

            if (!apiResponse.Success) return addresses;
            
            foreach (var novaPoshtaAddress in apiResponse.Data)
            {
                addresses.AddRange(novaPoshtaAddress.Addresses);
            }

            return addresses;
        }

        public async Task<List<NovaPoshtaSettlement>> GetAllSettlements()
        {
            var allData = new List<NovaPoshtaSettlement>();
            var page = 1;
            ApiResponse<NovaPoshtaSettlement> apiResponse;
            
            do
            {
                apiResponse = await NovaPoshtaApiGetData<NovaPoshtaSettlement, GetSettlementsProps>(props =>
                {
                    props.ModelName = "Address";
                    props.CalledMethod = "getSettlements";
                    props.Page = page;
                });

                if (!apiResponse.Success) break;

                allData.AddRange(apiResponse.Data);
                page++;
            } while (apiResponse.Data.Any());

            return allData;
        }

        public async Task<List<NovaPoshtaSettlement>> GetSettlementsByRef(string @ref)
        {
            var apiResponse = await NovaPoshtaApiGetData<NovaPoshtaSettlement, GetSettlementsProps>(props =>
            {
                props.ModelName = "Address";
                props.CalledMethod = "getSettlements";
                props.Ref = @ref;
            });

            return apiResponse.Success ? apiResponse.Data : new List<NovaPoshtaSettlement>();
        }

        public async Task<List<NovaPoshtaWarehouse>> GetAllWarehouses()
        {
            var apiResponse = await NovaPoshtaApiGetData<NovaPoshtaWarehouse, GetWarehousesProps>(props =>
            {
                props.ModelName = "AddressGeneral";
                props.CalledMethod = "getWarehouses";
                props.CityRef = null;
            });

            return apiResponse.Success ? apiResponse.Data : new List<NovaPoshtaWarehouse>();
        }

        public async Task<List<NovaPoshtaWarehouse>> GetWarehousesByCityRef(string cityRef)
        {
            var apiResponse = await NovaPoshtaApiGetData<NovaPoshtaWarehouse, GetWarehousesProps>(props =>
            {
                props.ModelName = "AddressGeneral";
                props.CalledMethod = "getWarehouses";
                props.CityRef = cityRef;
            });

            return apiResponse.Success ? apiResponse.Data : new List<NovaPoshtaWarehouse>();
        }

        public async Task<List<NovaPoshtaArea>> GetAllAreas()
        {
            var apiResponse = await NovaPoshtaApiGetData<NovaPoshtaArea, BaseRequestProps>(props =>
            {
                props.ModelName = "Address";
                props.CalledMethod = "getAreas";
            });

            return apiResponse.Success ? apiResponse.Data : new List<NovaPoshtaArea>();
        }

        public async Task<List<NovaPoshtaDocumentPrice>> GetDeliveryPrice(NovaPoshtaSettlement sender, 
            NovaPoshtaSettlement recipient, Product product, bool toWarehouse = true)
        {
            //TODO Check and recalculate dimensions
            
            var apiResponse = await NovaPoshtaApiGetData<NovaPoshtaDocumentPrice, GetDeliveryCostProps>(props =>
            {
                props.ModelName = "InternetDocument";
                props.CalledMethod = "getDocumentPrice";
                props.CitySender = sender.Ref;
                props.CityRecipient = recipient.Ref;
                props.Weight = ((int)product.Weight).ToString();
                props.ServiceType = toWarehouse ? "WarehouseWarehouse" : "WarehouseDoors";
                props.Cost = ((int)product.Price).ToString();
                props.CargoType = "Parcel";
                props.SeatsAmount = "1";
                props.OptionsSeat = new List<OptionSeat>
                {
                    new()
                    {
                        weight = ((int)product.Weight).ToString(),
                        volumetricHeight = ((int)(product.Height * 100)).ToString(),
                        volumetricLength = ((int)(product.Length * 100)).ToString(),
                        volumetricWidth = ((int)(product.Width * 100)).ToString()
                    }
                };
            });

            return apiResponse.Success ? apiResponse.Data : new List<NovaPoshtaDocumentPrice>();
        }

        private async Task<NovaPoshtaSettings> GetNovaPoshtaSettings()
        {
            return await _settingService.LoadSettingAsync<NovaPoshtaSettings>();
        }

        private async Task<string> ApiKey()
        {
            return (await GetNovaPoshtaSettings()).ApiKey;
        }

        private async Task<ApiResponse<TData>> NovaPoshtaApiGetData<TData, TRequest>(Action<TRequest> props)
            where TRequest : BaseRequestProps, new()
        {
            var settings = await GetNovaPoshtaSettings();
            var request = new ApiRequest<TRequest>(await ApiKey(), props);

            var httpClient = new HttpClient();
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            };

            var responseMessage = await httpClient.PostAsJsonAsync(settings.ApiUrl, request, jsonSerializerOptions);

            var stringResponse = await responseMessage.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TData>>(stringResponse, jsonSerializerOptions);

            return apiResponse;
        }
    }
}