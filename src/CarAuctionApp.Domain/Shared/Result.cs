namespace CarAuctionApp.Domain.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error!;
        }

        public static Result Success() => new Result(true, Error.None);

        public static Result Failure(Error error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(bool isSuccess, T value, Error error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) =>
            new Result<T>(true, value, Error.None);

        public static Result<T> Failure(Error error) =>
            new Result<T>(false, default!, error);
    }

}
