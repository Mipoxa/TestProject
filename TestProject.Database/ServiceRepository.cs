using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProject.ClassLogic;
using TestProject.Entities;

namespace TestProject.Database;

public class ServiceRepository : IServiceRepository
{
    private readonly TestProjectDbContext _dbContext;
    public ServiceRepository(TestProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Service>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        List<Guid> idList = ids.ToList();
        List<Service> services = await _dbContext.Services.Where(s => idList.Contains(s.Id)).ToListAsync(ct);
        return services;
    }
}