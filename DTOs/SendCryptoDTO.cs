namespace CryptoWalletAPI.DTOs
{
    public class SendCryptoDTO
    {
        public string RecipientEmail { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
    }
}
