using CryptoSphereStats.Domain.Enums;

namespace CryptoSphereStats.Domain.Models
{
    public class ChartData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Month Month { get; set; } 
        public decimal Value { get; set; }

        // Foreign key
        public int CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
    }
}
