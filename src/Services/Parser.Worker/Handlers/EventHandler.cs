using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EA.Domain.Abstraction.Repositories;
using EA.Domain.Entities;
using EA.Domain.Events;

namespace Parser.Worker.Handlers
{
    public interface IEventHandler
    {
        Task On(AddCategoryEvent @event);
    }

    public class EventHandler : IEventHandler
    {
        private readonly IRepository<Category> _categoryRepository;

        public EventHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task On(AddCategoryEvent @event)
        {
            var category = new Category()
            {

            };

            return _categoryRepository.CreateAsync(category);
        }
    }
}