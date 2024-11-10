using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoSphereStats.Domain.Models;

namespace CryptoSphereStats.Services
{
    public class CryptoService
    {
        private readonly HttpClient _httpClient;

        public CryptoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CryptoData>> GetCryptoDataAsync()
        {
            var cryptoApiUrl = "https://api.coinlore.net/api/tickers/";

            // Call the API and get the response
            var response = await _httpClient.GetStringAsync(cryptoApiUrl);

            // Deserialize the JSON response into the data model
            var result = JsonConvert.DeserializeObject<ApiResponse>(response);

            // Return the list of CryptoData objects
            return result?.Data ?? new List<CryptoData>();
        }
    }

    // API Response Structure
    public class ApiResponse
    {
        public List<CryptoData> Data { get; set; }
    }
}
