namespace Application.Commons.Exceptions
{
    public class BaseException(int statusCode, string title, string message) : Exception
    {
        public int StatusCode { get; set; } = statusCode;
        public string Title { get; set; } = title;
        public override string Message => message;
    }
}