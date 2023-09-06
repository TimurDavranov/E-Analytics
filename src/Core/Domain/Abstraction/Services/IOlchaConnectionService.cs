using Domain.DTOs.Responses.Olcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstraction.Services
{
    public interface IOlchaConnectionService
    {
        Task<OlchaBaseResponse<OlchaCategoryResponse<OlchaCategoriesDto>>> GetCategories();
    }
}
