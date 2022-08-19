namespace HotelWebApp.Exceptions;

/// <summary>
/// Пользовательское исключение для ошибок связаных с запросом пользователя
/// </summary>
public class RequestException : Exception
{
    /// <summary>
    /// Статусный код ответа
    /// </summary>
    public int StatusCode { get; private set; }
    
    /// <summary>
    /// Конструктор исключения
    /// </summary>
    /// <param name="message">Сообщение об ошибки</param>
    /// <param name="statuscode">Статусный код ответа</param>
    public RequestException(string message, int statuscode) : base(message)
    {
        StatusCode = statuscode;
    }
}