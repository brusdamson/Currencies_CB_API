using CurrenciesSERVICE_2._0.Data;
using CurrenciesSERVICE_2._0.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Xml.Linq;

namespace CurrenciesSERVICE_2._0.Services
{
    public class CurrencyService:BackgroundService
    {
        private readonly IMemoryCache memoryCache;
        private readonly CurrenciesServiceContext context;

        public CurrencyService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.context = CurrenciesServiceContext.GetContext();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                    XDocument xml = XDocument.Load("http://www.cbr.ru/scripts/XML_daily.asp");
                    List<CurrencyModel> currencyModels = new List<CurrencyModel>();
                    foreach (var item in xml.Descendants("Valute"))
                    {
                        currencyModels.Add(new CurrencyModel
                        {
                            CurrencyId = item.Attribute("ID").Value.ToString(),
                            Date = DateTime.Today,
                            CharValute = item.Element("CharCode").Value,
                            NominalValute = int.Parse(item.Element("Nominal").Value),
                            NameValute = item.Element("Name").Value,
                            Value = Convert.ToDecimal(item.Element("Value").Value)
                        });


                    }

                    //Если нет курса нулевой валюты с текущей датой, заполняем все курсы
                    if (!context.CurrencyModel.Any(o => o.Date == currencyModels[0].Date))
                    {
                        foreach (var curr in currencyModels)
                        {
                            context.Add(curr);
                            context.SaveChanges();
                        }
                    }

                    //Установка списка курса валют в кэш
                    memoryCache.Set("temp_currency", currencyModels, TimeSpan.FromSeconds(1440));


                }
                catch (Exception e)
                {

                    //Логи
                }
                await Task.Delay(3600000);
            }
        }
    }
}
