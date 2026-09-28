using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Entities;

namespace TestProject.ClassLogic;
public interface IBookingRepository
{
    Task<bool> HasOverlapAsync(Guid roomId, DateTime start, DateTime end, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
}