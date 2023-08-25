using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configurations
{
    public class AppConfig
    {
        public string OlchaBaseUrl { get; init; }
        public string OlchaGetCategoriesUrl { get; init; }
        public string OlchaProductsUrl { get; init; }
        public string OlchaGetProductsUrl(long categoryId) => OlchaProductsUrl + categoryId;
    }
}
