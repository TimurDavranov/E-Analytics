using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public class AppConfig
    {
        public string OlchaBaseUrl = "https://mobile.olcha.uz/api/v2";
        public string OlchaGetCategoriesUrl = "/categories";
        public string OlchaProductsUrl = "/products?category=";
        public string OlchaGetProductsUrl(long categoryId) => this.OlchaProductsUrl + categoryId;
    }
}
