using System.Security.Cryptography;
using System.Text;
using CryptoWalletAPI.Data;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Models;

namespace CryptoWalletAPI.Services
{
    public class UserService : IUserService
    {
        private readonly CryptoWalletContext _context;

        public UserService(CryptoWalletContext context)
        {
            _context = context;
        }

        public Result Register(UserRegisterDTO dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return Result.Failure("Email already in use.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Wallet = new Wallet
                {
                    BalanceUSD = 100000, // Set initial wallet balance
                    Cryptos = new List<CryptoHoldings>() // Initialize empty crypto list
                }
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Result.Success();
        }

        public User Authenticate(UserLoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || user.PasswordHash != HashPassword(dto.Password))
                return null;

            return user;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
