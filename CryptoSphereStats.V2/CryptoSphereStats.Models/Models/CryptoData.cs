using System;

namespace CryptoSphereStats.Domain.Models
{
    public class CryptoData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public double PriceUsd { get; set; } 
        public double MarketCap { get; set; }
        public double Volume24h { get; set; }
        public double PercentChange1h { get; set; }
        public double PercentChange24h { get; set; }
        public double PercentChange7d { get; set; }
        public int Rank { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
    }
}
