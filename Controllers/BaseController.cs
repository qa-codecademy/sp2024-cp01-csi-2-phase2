using CryptoWalletAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Response<TResult>(Result<TResult> response) where TResult : new()
        {
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response.Outcome);
        }

        protected IActionResult Response(Result response)
        {
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok();
        }
    }
}
