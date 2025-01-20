namespace CarAuctionApp.Domain.Shared
{
    public class Error
    {
        public static readonly Error None = new Error(string.Empty, string.Empty);
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string ToString() => $"{Code}: {Message}";
    }

}
