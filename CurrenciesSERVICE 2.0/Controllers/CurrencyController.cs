using CurrenciesSERVICE_2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using X.PagedList;

namespace CurrenciesSERVICE_2._0.Controllers
{
    public class CurrencyController:Controller
    {
        private readonly IMemoryCache memoryCache;
        private List<CurrencyModel> currency;
        public CurrencyController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            if (!memoryCache.TryGetValue("temp_currency", out currency))
            {
                throw new Exception("Ошибка чтения кэша!");
            }


        }
        // GET: Currency
        public IActionResult Index(int page = 1)
        {
            var values = currency.ToPagedList(page, 5);
            return View(values);
        }
    }
}
