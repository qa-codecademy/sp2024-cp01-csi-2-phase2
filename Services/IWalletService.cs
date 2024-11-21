using System.Threading.Tasks;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Models;

namespace CryptoWalletAPI.Services
{
    public interface IWalletService
    {
        Task<Result<Wallet>> GetWallet(int userId);
        Task<Result> BuyCrypto(CryptoTransactionDTO dto, int userId);
        Task<Result>  SellCrypto(CryptoTransactionDTO dto, int userId);
        Task<Result> SendCrypto(SendCryptoDTO dto,int receiverId, int senderId);
        Task<Result<decimal>> GetCryptoPrice(string symbol);
        Task<Result> CryptoToCrypto(CryptoToCryptoDTO dto,int userId);
        Task<List<CryptoData>> GetCryptoDataAsync();
    }
}
