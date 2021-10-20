using System;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class ApiRequest<T> where T : BaseRequestProps
    {
        public string ModelName;
        public string CalledMethod;
        public string ApiKey;
        public T MethodProperties;

        public ApiRequest(string apiKey, Action<T> properties)
        {
            ApiKey = apiKey;
            properties.Invoke(MethodProperties);
            ModelName = MethodProperties.ModelName;
            CalledMethod = MethodProperties.CalledMethod;
        }
    }
}