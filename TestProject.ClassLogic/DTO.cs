using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Entities;

namespace TestProject.ClassLogic;
//DTO - класи


//Розрахунок вартості оренди (CalculatePriceAsync) - вхідні дані
public record CalculatePriceRequest( 
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    List<Guid> ServiceIds
);
//Додавання конференц-залу (CreateRoomAsync) - вхідні дані
public record CreateRoomRequest(
    string Name,
    int Size,
    decimal Price,
    List<Guid> ServiceIds
);
//Редагування інформації про зал (UpdateRoomAsync) - вхідні дані
public record UpdateRoomRequest(
    string Name,
    int Size,
    decimal Price,
    List<Guid> ServiceIds
);
//Пошук доступних залів (GetAvailableRoomsAsync) - вхідні дані
public record SearchAvailableRoomsRequest(
    DateTime StartTime,
    DateTime EndTime,
    int MinimumSize
);
//Додавання/редагування/пошук залів (CreateRoomAsync, UpdateRoomAsync, GetAvailableRoomsAsync) - вихідні дані
public record RoomResponseDto(
    Guid Id,
    string Name,
    int Size,
    decimal Price
);
//Бронювання залу (CreateBookingAsync) - вхідні дані
public record CreateBookingRequest(
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    List<Guid> ServiceIds
);
//Бронювання залу (CreateBookingAsync) - вихідні дані
public record BookingResponseDto(
    Guid BookingId,
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    decimal Price
);