namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение, вызываемое при ошибки проверки пароля, наследуется от RequestException
/// </summary>
public class PasswordValidationException : RequestException
{
    public PasswordValidationException() : base("Пароль введен неверно", 400) {}
}