using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Models;

namespace CryptoWalletAPI.Services
{
    public interface IUserService
    {
        Result Register(UserRegisterDTO dto);
        User Authenticate(UserLoginDTO dto);
    }

}
