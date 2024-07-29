using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.DataService.IRepository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthTracker.DataService.Repository;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected AppDbContext _context;
    internal DbSet<T> dbset;
    protected readonly ILogger _logger;
    public GenericRepository(AppDbContext context, ILogger logger)
    {
        _context = context;
        dbset = context.Set<T>();
        _logger = logger;
    }
    public virtual async Task<bool> Add(T entity)
    {
        await dbset.AddAsync(entity);
        return true;
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await dbset.ToListAsync();
    }

    public virtual async Task<bool> Delete(Guid id, string userId)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await dbset.FindAsync(id);
    }

    public Task<bool> Upsert(T entity)
    {
        throw new NotImplementedException();
    }
}
