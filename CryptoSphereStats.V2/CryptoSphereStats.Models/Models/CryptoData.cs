using Newtonsoft.Json;
using System;

namespace CryptoSphereStats.Domain.Models
{
    public class CryptoData
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string PriceUsd { get; set; }
        public string PercentChange1h { get; set; }
        public string PercentChange24h { get; set; }
        public string PercentChange7d { get; set; }
        public decimal? MarketCapUsd { get; set; }
        public decimal? Volume24 { get; set; }
        public decimal? Csupply { get; set; }
        public decimal? Tsupply { get; set; }
        public decimal? Msupply { get; set; }
    }
}
