namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение, возникающее при неправильном вводе дат
/// </summary>
public class DatesValidationException : RequestException
{
    public DatesValidationException() : base("Даты введены неверно", 400) {}
}