using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuySellRequest
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
    }
}
