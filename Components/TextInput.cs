using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Shipping.NovaPoshta.Components
{
    public class TextInput : ViewComponent
    {
        public IViewComponentResult Invoke(string title, string id = "", int titleWidthPercent = 40, string value = "", bool disabled = false)
        {
            var textInputModel = new TextInputModel
            {
                Id = string.IsNullOrEmpty(id) ? DateTime.Now.ToBinary().ToString() : id,
                Title = title,
                Value = value,
                Disabled = disabled,
                TitleWidthPercent = titleWidthPercent
            };
            
            return View(textInputModel);
        }
    }
}