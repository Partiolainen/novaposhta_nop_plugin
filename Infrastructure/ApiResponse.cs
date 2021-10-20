using System.Collections.Generic;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }

        public List<TData> Data { get; set; } = new();

        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> MessageCodes { get; set; }
        public List<string> ErrorCodes { get; set; }
        public List<string> WarningCodes { get; set; }
        public List<string> InfoCodes { get; set; }
        
    }
}