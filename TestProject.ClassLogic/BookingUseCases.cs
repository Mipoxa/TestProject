using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;
using TestProject.Entities;

namespace TestProject.ClassLogic;
public interface IBookingUseCases
{
    Task<Decimal> CalculatePriceAsync(CalculatePriceRequest request, CancellationToken ct = default);
    Task<BookingResponseDto> CreateBookingAsync(CreateBookingRequest request, CancellationToken ct = default);
}
public class BookingUseCases : IBookingUseCases
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ITimeZoneCalculator _pricingService;
    public BookingUseCases(
        IRoomRepository roomRepository,
        IBookingRepository bookingRepository,
        IServiceRepository serviceRepository,
        ITimeZoneCalculator pricingService)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _serviceRepository = serviceRepository;
        _pricingService = pricingService;
    }

    public async Task<decimal> CalculatePriceAsync(CalculatePriceRequest request, CancellationToken ct = default) //Викликає калькулятор щоб розрахувати вартість за час, додає суму вартості послуг та повертає.
    {
        Room? room = await _roomRepository.GetByIdAsync(request.RoomId, ct);
        if (room == null)
        {
            throw new InvalidOperationException("No room with ID " + request.RoomId + " found!");
        }
        double hours = (request.EndTime - request.StartTime).TotalHours;
        if (hours <= 0)
        {
            throw new ArgumentException("Invalid Times!");
        }
        room.AreServicesAvailable(request.ServiceIds);
        decimal roomPrice = _pricingService.CalculateRoomPrice(room.Price, request.StartTime, request.EndTime);
        IEnumerable<Service> services = await _serviceRepository.GetByIdsAsync(request.ServiceIds, ct);
        decimal servicesPrice = services.Sum(s => s.Price);
        return roomPrice + servicesPrice;
    }
    public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingRequest request, CancellationToken ct = default) //Перевіряє зайнятість залу, якщо не зайнятий, рахує ціну, бронює, та повертає інформацію про бронювання.
    {
        bool isOccupied = await _bookingRepository.HasOverlapAsync(request.RoomId, request.StartTime, request.EndTime, ct);
        if (isOccupied)
        {
            throw new InvalidOperationException("Room already booked!");
        }
        IEnumerable<Service> services = await _serviceRepository.GetByIdsAsync(request.ServiceIds, ct);
        CalculatePriceRequest priceRequest = new CalculatePriceRequest(request.RoomId, request.StartTime, request.EndTime, request.ServiceIds);
        decimal totalPrice = await CalculatePriceAsync(priceRequest, ct);
        Booking booking = new Booking(request.RoomId, request.StartTime, request.EndTime, totalPrice, services);
        await _bookingRepository.AddAsync(booking, ct);
        return new BookingResponseDto(booking.Id, booking.RoomId, booking.StartTime, booking.EndTime, booking.Price);
    }
}