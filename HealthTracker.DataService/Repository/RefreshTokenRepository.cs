using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.DataService.IRepository;
using HealthTracker.Entities.DbSet;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthTracker.DataService.Repository;
public class RefreshTokenRepository:GenericRepository<RefreshToken>,IRefreshTokenRepository
{

    public RefreshTokenRepository(AppDbContext context,ILogger logger):base(context, logger)
    {
        
    }

    public override async Task<IEnumerable<RefreshToken>> All()
    {
        try
        {
            return await dbset.Where(x => x.Status == 1)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} all method has generated an error", typeof(RefreshTokenRepository));

            return new List<RefreshToken>();
        }
    }
}
