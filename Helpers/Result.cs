namespace CryptoWalletAPI.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true);
        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }

}

