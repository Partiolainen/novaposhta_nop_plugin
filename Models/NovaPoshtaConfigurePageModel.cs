namespace Nop.Plugin.Shipping.NovaPoshta.Models
{
    public class NovaPoshtaConfigurePageModel
    {
        public bool DataBaseUpdateStarted { get; set; }

        public NovaPoshtaConfigurePageModel()
        {
        }

        public NovaPoshtaConfigurePageModel(bool dataBaseUpdateStarted = false)
        {
            DataBaseUpdateStarted = dataBaseUpdateStarted;
        }
    }
}