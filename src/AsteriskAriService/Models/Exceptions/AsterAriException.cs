namespace AsteriskAriService.Models.Exceptions
{
    public class AsterAriException : Exception
    {
        public int? StatusCode { get; set; }

        public AsterAriException(string message) : base(message)
        {
        }

        public AsterAriException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }

}