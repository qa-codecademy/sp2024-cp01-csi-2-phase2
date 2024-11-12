using System.Threading.Tasks;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Models;

namespace CryptoWalletAPI.Services
{
    public interface IWalletService
    {
        Wallet GetWallet(int userId);
        Result AddCrypto(CryptoTransactionDTO dto, int userId);
        Result BuyCrypto(CryptoTransactionDTO dto, int userId);
        Result SellCrypto(CryptoTransactionDTO dto, int userId);
        Result SendCrypto(SendCryptoDTO dto, int senderId);
        Task<decimal> GetCryptoPrice(string symbol);
    }
}
