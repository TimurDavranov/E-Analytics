using Domain.DTOs.Responses.Olcha;

namespace Application.Abstractions
{
    public interface IOlchaParserRepository
    {
        Task ParseCategories();
    }
}
