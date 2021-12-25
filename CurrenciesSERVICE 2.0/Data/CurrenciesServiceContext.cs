using CurrenciesSERVICE_2._0.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrenciesSERVICE_2._0.Data
{
    public class CurrenciesServiceContext:DbContext
    {
        private static CurrenciesServiceContext context;
        public CurrenciesServiceContext(DbContextOptions<CurrenciesServiceContext> options) : base(options) { }
        public static CurrenciesServiceContext GetContext()
        {
            if (context == null)
            {
                DbContextOptionsBuilder<Data.CurrenciesServiceContext> optionsBuilder = new DbContextOptionsBuilder<Data.CurrenciesServiceContext>();
                optionsBuilder.UseSqlServer(ConnStr.GetConnStr());
                DbContextOptions<Data.CurrenciesServiceContext> options = optionsBuilder.Options;
                context = new CurrenciesServiceContext(options);
            }
            return context;
        }
        public DbSet<CurrencyModel> CurrencyModel { get; set; }
        public DbSet<Person> Persons { get; set; }
    }
}
