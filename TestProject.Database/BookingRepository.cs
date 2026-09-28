using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProject.ClassLogic;
using TestProject.Entities;

namespace TestProject.Database;

public class BookingRepository : IBookingRepository
{
    private readonly TestProjectDbContext _dbContext;
    public BookingRepository(TestProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> HasOverlapAsync(Guid roomId, DateTime start, DateTime end, CancellationToken ct = default)
    {
        bool hasOverlap = await _dbContext.Bookings.AnyAsync(b => b.RoomId == roomId && start < b.EndTime && b.StartTime < end, ct);
        return hasOverlap;
    }
    public async Task AddAsync(Booking booking, CancellationToken ct = default)
    {
        await _dbContext.Bookings.AddAsync(booking, ct);
        await _dbContext.SaveChangesAsync(ct);
    }
}