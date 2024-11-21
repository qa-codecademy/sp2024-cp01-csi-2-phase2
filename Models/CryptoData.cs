using System.Text.Json.Serialization;

namespace CryptoWalletAPI.Models
{
    public class CryptoData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price_usd")]
        public string PriceUSD { get; set; }

        [JsonPropertyName("percent_change_1h")]
        public string PercentChange1h { get; set; }

        [JsonPropertyName("percent_change_24h")]
        public string PercentChange24h { get; set; }

        [JsonPropertyName("percent_change_7d")]
        public string PercentChange7d { get; set; }
    }



}
