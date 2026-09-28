using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Entities;

namespace TestProject.ClassLogic;
public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}