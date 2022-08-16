namespace HotelWebApp.Workers;

/// <summary>
/// Интерфейс сервиса для вызова методов из другого класс
/// </summary>
public interface IMethodCallService
{
    Task DoWork();
}