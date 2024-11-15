using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Services;
using CryptoWalletAPI.Controllers;
using CryptoWalletAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IUserService _userService;
    private readonly JwtHelper _jwtHelper;

    public AuthController(IUserService userService, JwtHelper jwtHelper)
    {
        _userService = userService;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserRegisterDTO registerDto)
    {
        try
        {
            var response = await _userService.Register(registerDto);
            return Response(response);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDTO loginDto)
    {
        try
        {
            var response = await _userService.Authenticate(loginDto);
            if (response == null) return BadRequest(response.ErrorMessage.ToString());

            var user = new User
            {
                Email = loginDto.Email,
                PasswordHash = loginDto.Password
            };
            var token = _jwtHelper.GenerateJwtToken(user);
            return Ok(token);
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }
}
