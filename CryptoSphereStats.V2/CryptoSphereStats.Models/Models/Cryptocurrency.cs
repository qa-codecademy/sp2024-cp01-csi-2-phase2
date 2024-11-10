using System.Text.Json.Serialization;

namespace CryptoSphereStats.Domain.Models
{
    public class Cryptocurrency
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<ChartData> ChartData { get; set; } = new List<ChartData>();
    }
}