using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Models;

namespace CryptoWalletAPI.Services
{
    public interface IUserService
    {
        Task<Result> Register(UserRegisterDTO dto);
        Task<Result> Authenticate(UserLoginDTO dto);
    }

}
