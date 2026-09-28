using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProject.ClassLogic;
using TestProject.Entities;

namespace TestProject.Database;

public class RoomRepository : IRoomRepository
{
    private readonly TestProjectDbContext _dbContext;
    public RoomRepository(TestProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Rooms.Include(r => r.AvailableServices).FirstOrDefaultAsync(r => r.Id == id, ct);
    }
    public async Task AddAsync(Room room, CancellationToken ct = default)
    {
        await _dbContext.Rooms.AddAsync(room, ct);
        await _dbContext.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(Room room, CancellationToken ct = default)
    {
        _dbContext.Rooms.Update(room);
        await _dbContext.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        Room? room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room is null)
        {
            return;
        }
        _dbContext.Rooms.Remove(room);
        await _dbContext.SaveChangesAsync(ct);
    }
    public async Task<IEnumerable<Room>> GetByCapacityAsync(int minimumSize, CancellationToken ct)
    {
        return await _dbContext.Rooms.Where(r => r.Size >= minimumSize).ToListAsync(ct);
    }
}