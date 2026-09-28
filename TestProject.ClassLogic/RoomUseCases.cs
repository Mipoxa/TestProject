using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Entities;

namespace TestProject.ClassLogic;

public interface IRoomUseCases
{
    Task<RoomResponseDto> CreateRoomAsync(CreateRoomRequest request, CancellationToken ct = default); //Створює сутність кімнати
    Task UpdateRoomAsync(Guid roomId, UpdateRoomRequest request, CancellationToken ct = default); //Оновлює сутність кімнати
    Task DeleteRoomAsync(Guid roomId, CancellationToken ct = default); //Видаляє сутність кімнати
    Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync (SearchAvailableRoomsRequest request, CancellationToken ct = default); //Шукає кімнати
}

public class RoomUseCases : IRoomUseCases
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;

    public RoomUseCases(IRoomRepository roomRepository, IBookingRepository bookingRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
    }
    public async Task<RoomResponseDto> CreateRoomAsync(CreateRoomRequest request, CancellationToken ct = default) //Створює і повертає кімнату
    {
        Room room = new Room(request.Name, request.Size, request.Price);
        await _roomRepository.AddAsync(room, ct);
        return new RoomResponseDto(room.Id, room.Name, room.Size, room.Price);
    }
    public async Task UpdateRoomAsync(Guid roomId, UpdateRoomRequest request, CancellationToken ct = default) //Оновлює і повертає кімнату
    {
        Room? room = await _roomRepository.GetByIdAsync(roomId, ct);
        if (room == null)
        {
            throw new InvalidOperationException("No room with ID " + roomId + " found!");
        }
        room.Update(request.Name, request.Size, request.Price);
        await _roomRepository.UpdateAsync(room, ct);
    }
    public async Task DeleteRoomAsync(Guid roomId, CancellationToken ct = default) //Видаляє кімнату
    {
        Room? room = await _roomRepository.GetByIdAsync(roomId, ct);
        if (room == null)
        {
            throw new InvalidOperationException("No room with ID " + roomId + " found!");
        }
        await _roomRepository.DeleteAsync(roomId, ct);
    }
    public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync(SearchAvailableRoomsRequest request, CancellationToken ct = default) //Шукає валідні кімнати, проходить через них, повертає незайняті.
    {
        IEnumerable<Room> rooms = await _roomRepository.GetByCapacityAsync(request.MinimumSize, ct); 
        List<Room> availableRooms = new List<Room>();
        foreach (Room room in rooms)
        {
            bool isOccupied = await _bookingRepository.HasOverlapAsync(room.Id, request.StartTime, request.EndTime, ct);
            if (!isOccupied)
            {
                availableRooms.Add(room);
            }
        }
        return availableRooms.Select(r => new RoomResponseDto(r.Id, r.Name, r.Size, r.Price));
    }
}