using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EAnalytics.Common.Factories
{
    public class DatabaseContextFactory<T> where T : DbContext
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;
        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public T CreateContext()
        {
            DbContextOptionsBuilder<T> optionsBuilder = new();
            _configureDbContext(optionsBuilder);

            Type anon = typeof(T);
            var instance = (T)Activator.CreateInstance(anon, new object[] { optionsBuilder.Options })!;
            return instance;
        }
    }
}
