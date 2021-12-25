using System.ComponentModel.DataAnnotations;

namespace CurrenciesSERVICE_2._0.Models
{
    public class CurrencyModel
    {
        [Key]
        public int Id { get; set; }
        public string CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public string CharValute { get; set; }
        public int NominalValute { get; set; }
        public string NameValute { get; set; }
        public decimal Value { get; set; }
    }
}
