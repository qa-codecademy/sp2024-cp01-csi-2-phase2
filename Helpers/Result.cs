namespace CryptoWalletAPI.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; set; } = false;
        public IEnumerable<string> ErrorMessage { get;  set; } = new List<string>();

        public static Result Success => new(true);
        public Result(params string[] errors) => ErrorMessage = errors;
        public Result(IEnumerable<string> errors) => ErrorMessage = errors;

        public Result(bool IsSuccess) => IsSuccess = IsSuccess;
    }

    public class Result<TResult> : Result where TResult : new()
    {
        public TResult? Outcome { get; set; }
        public Result(TResult? outcome)
        {
            IsSuccess = true;
            Outcome = outcome;
        }
        public Result (params string[] errors) : base(errors) { }   
        public Result(IEnumerable<string> errors): base(errors) { }
    }

}

