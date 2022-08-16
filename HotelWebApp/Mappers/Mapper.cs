using HotelWebApp.Models;

namespace HotelWebApp.Mappers;

/// <summary>
/// Класса для преобразования объекта из <c>User</c> в <c>UserDTO</c>
/// </summary>
public static class Mapper
{
    /// <summary>
    /// Метод для преобразования объекта из <c>User</c> в <c>UserDTO</c>
    /// </summary>
    /// <param name="user">Объект класса <c>User</c></param>
    /// <returns>Объект класса <c>UserDTO</c></returns>
    public static UserDTO ToUserDTO(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            RegisteredAt = user.RegisteredAt,
            Role = user.Role
        };
    }
}