using System;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class ApiRequest<T> where T : BaseRequestProps, new()
    {
        public string modelName;
        public string calledMethod;
        public string apiKey;
        public readonly T methodProperties = new();

        public ApiRequest(string apiKey, Action<T> properties)
        {
            this.apiKey = apiKey;
            properties(methodProperties);
            modelName = methodProperties.ModelName;
            calledMethod = methodProperties.CalledMethod;
        }
    }
}