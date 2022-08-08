namespace HotelWebApp;

/// <summary>
/// Интерфейс для инициализатора БД
/// </summary>
public interface IInitializer
{
    /// <summary>
    /// Метод для инициализации БД, принимает контекст подключения к БД
    /// </summary>
    /// <param name="context">Контекст подключения к БД</param>
    void Initialize(ApplicationContext context);
}