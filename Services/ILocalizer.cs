using System.Threading.Tasks;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface ILocalizer
    {
        Task SetLocaleResources();
        Task RemoveLocaleResources();
    }
}