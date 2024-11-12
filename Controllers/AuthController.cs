using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtHelper _jwtHelper;

    public AuthController(IUserService userService, JwtHelper jwtHelper)
    {
        _userService = userService;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("register")]
    public IActionResult Register(UserRegisterDTO registerDto)
    {
        var result = _userService.Register(registerDto);
        return result.IsSuccess
            ? Ok(new { message = "User registered successfully", status = "success" })
            : BadRequest(new { message = result.ErrorMessage });
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDTO loginDto)
    {
        var user = _userService.Authenticate(loginDto);
        if (user == null) return Unauthorized(new { message = "Invalid credentials" });

        var token = _jwtHelper.GenerateJwtToken(user);
        return Ok(new { message = "Login successful", token = token, status = "success" });
    }
}
