namespace CryptoWalletAPI.DTOs
{
    public class AddTransactionDto
    {
        public int SenderWalletId { get; set; }
        public int? RecieverWalletId { get; set; }
        public string CoinSymbol { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
    }
}
