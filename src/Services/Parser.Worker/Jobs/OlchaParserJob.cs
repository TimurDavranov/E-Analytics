using Domain.Abstraction.Repositories.Olcha;

namespace Parser.Worker.Jobs
{
    public interface IOlchaParserJob
    {
        Task Run();
    }

    public class OlchaParserJob : IOlchaParserJob
    {
        private readonly IOlchaCategoryWriteRepository _repository;
        public OlchaParserJob(IOlchaCategoryWriteRepository repository)
        {
            _repository = repository;
        }

        public Task Run() =>
            _repository.ParseCategories();
    }
}
