namespace HotelWebApp.Exceptions;

public class PasswordValidationException : RequestException
{
    public PasswordValidationException() : base("Пароль введен неверно", 400) {}
}