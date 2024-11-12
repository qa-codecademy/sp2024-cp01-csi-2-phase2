using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    private int GetUserId()
    {
        if (HttpContext.User.FindFirst("userId")?.Value is string userIdStr && int.TryParse(userIdStr, out int userId))
        {
            return userId;
        }
        throw new UnauthorizedAccessException("User is not authenticated.");
    }

    [HttpGet]
    public IActionResult GetWallet()
    {
        var userId = GetUserId();
        var wallet = _walletService.GetWallet(userId);
        return Ok(wallet);
    }

    [HttpPost("add-crypto")]
    public IActionResult AddCrypto(CryptoTransactionDTO transactionDto)
    {
        var userId = GetUserId();
        var result = _walletService.AddCrypto(transactionDto, userId);
        return result.IsSuccess
            ? Ok(new { message = "Cryptocurrency added successfully", status = "success" })
            : BadRequest(new { message = result.ErrorMessage });
    }

    [HttpPost("buy")]
    public IActionResult BuyCrypto(CryptoTransactionDTO transactionDto)
    {
        var userId = GetUserId();
        var result = _walletService.BuyCrypto(transactionDto, userId);
        return result.IsSuccess
            ? Ok(new { message = "Cryptocurrency bought successfully", status = "success" })
            : BadRequest(new { message = result.ErrorMessage });
    }

    [HttpPost("sell")]
    public IActionResult SellCrypto(CryptoTransactionDTO transactionDto)
    {
        var userId = GetUserId();
        var result = _walletService.SellCrypto(transactionDto, userId);
        return result.IsSuccess
            ? Ok(new { message = "Cryptocurrency sold successfully", status = "success" })
            : BadRequest(new { message = result.ErrorMessage });
    }

    [HttpPost("send")]
    public IActionResult SendCrypto(SendCryptoDTO sendDto)
    {
        var senderId = GetUserId();
        var result = _walletService.SendCrypto(sendDto, senderId);
        return result.IsSuccess
            ? Ok(new { message = "Cryptocurrency sent successfully", status = "success" })
            : BadRequest(new { message = result.ErrorMessage });
    }

    [HttpGet("crypto-price/{symbol}")]
    public async Task<IActionResult> GetCryptoPrice(string symbol)
    {
        var price = await _walletService.GetCryptoPrice(symbol);
        return Ok(new { symbol, price });
    }
}
