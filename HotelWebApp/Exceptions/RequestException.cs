namespace HotelWebApp.Exceptions;

public class RequestException : Exception
{
    public int StatusCode { get; private set; }
    
    public RequestException(string message, int statuscode) : base(message)
    {
        StatusCode = statuscode;
    }
}