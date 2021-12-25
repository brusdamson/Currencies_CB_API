using System.ComponentModel.DataAnnotations;

namespace CurrenciesSERVICE_2._0.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
