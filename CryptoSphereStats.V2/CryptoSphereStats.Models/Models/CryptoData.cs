<<<<<<< HEAD
﻿using Newtonsoft.Json;
using System;
=======
﻿using CryptoSphereStats.Domain.Enums;
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c

namespace CryptoSphereStats.Domain.Models
{
    public class CryptoData
    {
<<<<<<< HEAD
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
=======
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;


>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
    }
}
