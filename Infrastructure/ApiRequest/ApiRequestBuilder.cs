using System;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class ApiRequestBuilder
    {
        private readonly string _apiKey;

        public ApiRequestBuilder(string apiKey)
        {
            _apiKey = apiKey;
        }

        public ApiRequest<T> Build<T>(Action<T> properties) where T : BaseRequestProps, new()
        {
            var apiRequest = new ApiRequest<T>(_apiKey, properties);

            return apiRequest;
        }
    }
}