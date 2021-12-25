using CurrenciesSERVICE_2._0.Data;
using CurrenciesSERVICE_2._0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrenciesSERVICE_2._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly CurrenciesServiceContext _context;
        private readonly IDistributedCache distributedCache;
        public CurrenciesController(IDistributedCache distributedCache)
        {
            _context = CurrenciesServiceContext.GetContext();
            this.distributedCache = distributedCache;

        }
        // GET: api/<CurrenciesController>
        
        [Route("GetCurrencies")]
        [HttpGet]
        public async Task<IEnumerable<CurrencyModel>> GetCurrencies()
        {
            IEnumerable<CurrencyModel> currencies = new List<CurrencyModel>();
            //distributedCache.Remove("currencies");
            if (string.IsNullOrEmpty(await distributedCache.GetStringAsync("currencies")))
            {
                currencies = await _context.CurrencyModel.ToListAsync();

                var currenciesInString = JsonConvert.SerializeObject(currencies);
                
                await distributedCache.SetStringAsync("currencies",currenciesInString, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)});
            }
            else
            {
                var currenciesFromCache = await distributedCache.GetStringAsync("currencies");
                if (currenciesFromCache != null)
                    currencies = JsonConvert.DeserializeObject<List<CurrencyModel>>(currenciesFromCache);
                else return null;
            }
            return currencies;
        }

        // GET api/<CurrenciesController>/5
        [Route("GetByDate")]
        [HttpGet("GetByDate/{date}")]
        public async Task<IEnumerable<CurrencyModel>> GetByDate(string date)
        {
            IEnumerable<CurrencyModel> currencies = new List<CurrencyModel>();
            //distributedCache.Remove("currencies");
            if (!string.IsNullOrEmpty(date))
            {
                if (string.IsNullOrEmpty(await distributedCache.GetStringAsync("currencies")))
                {
                    currencies = await _context.CurrencyModel.ToListAsync();

                    var currenciesInString = JsonConvert.SerializeObject(currencies);

                    await distributedCache.SetStringAsync("currencies", currenciesInString, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });
                }
                else
                {
                    var currenciesFromCache = await distributedCache.GetStringAsync("currencies");
                    if (currenciesFromCache != null)
                        currencies = JsonConvert.DeserializeObject<List<CurrencyModel>>(currenciesFromCache);
                    else return null;
                }
                try
                {
                    //Доделать обработать эксепшн
                    return currencies.Where(p => p.Date == DateTime.ParseExact(date.Replace("GetByDate&date=", "").Replace("\'", ""), "dd:MM:yyyy", null));
                }
                catch (Exception ex)
                {
                    return null;
                }
                
            }
            return null;
        }
        [Authorize]
        [Route("GetById")]
        [HttpGet("GetById/{id}")]
        //Пример - https://localhost:7181/api/Currencies/GetById/GetById&id=R01010
        public async Task<CurrencyModel> GetById(string id)
        {
            IEnumerable<CurrencyModel> currencies = new List<CurrencyModel>();
            //distributedCache.Remove("currencies");
            if (!string.IsNullOrEmpty(id))
            {
                if (string.IsNullOrEmpty(await distributedCache.GetStringAsync("currencies")))
                {
                    currencies = await _context.CurrencyModel.ToListAsync();

                    var currenciesInString = JsonConvert.SerializeObject(currencies);

                    await distributedCache.SetStringAsync("currencies", currenciesInString, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });
                }
                else
                {
                    var currenciesFromCache = await distributedCache.GetStringAsync("currencies");
                    if (currenciesFromCache != null)
                        currencies = JsonConvert.DeserializeObject<List<CurrencyModel>>(currenciesFromCache);
                    else return null;
                }
                try
                {
                    //Доделать обработать эксепшн
                    return currencies.Where(p => p.CurrencyId == id.Replace("GetById&id=","")).Where(p => p.Date == DateTime.Today).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return null;
        }
        
    }
}
