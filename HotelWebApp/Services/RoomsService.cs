using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Interfaces.Services;
using HotelWebApp.Models;
using HotelWebApp.Repositories;

namespace HotelWebApp.Services;

public class RoomsService : IRoomsService
{
    /// <summary>
    /// Интерфейс репозитория для работы с комнатами
    /// </summary>
    private readonly IRoomsRepository _roomsRepository;

    public RoomsService(IRoomsRepository roomsRepository)
    {
        _roomsRepository = roomsRepository;
    }

    /// <inheritdoc cref="IRoomsService.GetAll(RoomFilter filter)"/>
    public async Task<List<HotelRoom>> GetAll(RoomFilter filter)
    {
        return await _roomsRepository.GetAll(filter);
    }

    /// <inheritdoc cref="IRoomsService.GetById(int roomId)"/>
    public async Task<HotelRoom> GetById(int roomId)
    {
        var room = await _roomsRepository.GetById(roomId);

        if (room == null) throw new RoomNotFoundException();

        return room;
    }

    /// <inheritdoc cref="IRoomsService.GetFreeRooms(BookingFilter filter)"/>
    public async Task<List<HotelRoom>> GetFreeRooms(BookingFilter filter)
    {
        if (filter.StartAt >= filter.FinishAt || filter.StartAt < DateTime.Now)
        {
            throw new DatesValidationException();
        }
        
        return await _roomsRepository.GetFreeRooms(filter);
    }
}