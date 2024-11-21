using CryptoWalletAPI.Controllers;
using CryptoWalletAPI.Data;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : BaseController
{
    private readonly IWalletService _walletService;
    private readonly CryptoWalletContext _context;

    public WalletController(IWalletService walletService, CryptoWalletContext context)
    {
        _walletService = walletService;
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetWallet()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return BadRequest("No user identified");
            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userIdClaim);
            var userId = findUser.Id;
            var wallet = await _walletService.GetWallet(userId);
            return Ok(wallet);
        }
        catch(Exception ex)
        { throw new Exception(ex.Message);
        }
    }

    [HttpPost("buy-crypto")]
    public async Task<IActionResult> BuyCrypto(CryptoTransactionDTO transactionDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return BadRequest("No user identified");
            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userIdClaim);
            var userId = findUser.Id;
            var result = await _walletService.BuyCrypto(transactionDto, userId);

            return result.IsSuccess
                ? Ok(new { message = "Cryptocurrency bought successfully", status = "success" })
                : BadRequest(new { message = result.ErrorMessage });
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("sell-crypto")]
    public async Task<IActionResult> SellCrypto(CryptoTransactionDTO transactionDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return BadRequest("No user identified");
            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userIdClaim);
            var userId = findUser.Id;
            var result = await _walletService.SellCrypto(transactionDto, userId);

            return result.IsSuccess
                    ? Ok(new { message = "Cryptocurrency sold successfully", status = "success" })
                    : BadRequest(new { message = result.ErrorMessage });
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("send-crypto")]
    public async Task<IActionResult> SendCrypto([FromBody]SendCryptoDTO sendDto)
    {
        try
        {
            //Find users id
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return BadRequest("No user identified");
            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userIdClaim);
            var senderId = findUser.Id;

            //find the id of Receiver through inputted email
            var findReceiver = await _context.Users.FirstOrDefaultAsync(x => x.Email == sendDto.RecipientEmail);
            var receiverId = findReceiver.Id;

            var result = await _walletService.SendCrypto(sendDto, receiverId, senderId);
            return result.IsSuccess
                ? Ok(new { message = "Cryptocurrency sent successfully", status = "success" })
                : BadRequest(new { message = result.ErrorMessage });
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("crypto-price/{symbol}")]
    public async Task<IActionResult> GetCryptoPrice(string symbol)
    {
        try
        {
            var price = await _walletService.GetCryptoPrice(symbol);
            return Ok(new { symbol, price });
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("exchange-crypto")]
    public async Task<IActionResult> ExchangeCrypto (CryptoToCryptoDTO dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return BadRequest("No user identified");
        var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userIdClaim);
        var userId = findUser.Id;

        var result = await _walletService.CryptoToCrypto(dto, userId);
        return result.IsSuccess
            ? Ok(new { message = "Cryptocurrency exchanged successfully", status = "success" })
            : BadRequest(new { message = result.ErrorMessage });
    }

}
