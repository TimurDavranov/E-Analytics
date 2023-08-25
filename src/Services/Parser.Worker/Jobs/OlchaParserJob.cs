using Application.Abstractions;

namespace Parser.Worker.Jobs
{
    public interface IOlchaParserJob
    {
        Task Run();
    }

    public class OlchaParserJob : IOlchaParserJob
    {
        private readonly IOlchaParserRepository _repository;
        public OlchaParserJob(IOlchaParserRepository repository)
        {
            _repository = repository;
        }

        public async Task Run()
        {
            var categories = await _repository.ParseCategories();
            
        }
    }
}
