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
public class HealthDataRepository:GenericRepository<HealthData>,IHealthDataRepository
{

    public HealthDataRepository(AppDbContext context,ILogger logger):base(context, logger)
    {
        
    }


    public override async Task<IEnumerable<HealthData>> All()
    {
        try
        {
            return await dbset.Where(x => x.Status == 1)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} all method has generated an error", typeof(HealthDataRepository));

            return new List<HealthData>();
        }
    }

    public async Task<bool> UpdateHealthData(HealthData healthData)
    {
        try
        {
            var existingHealthData = await dbset.Where(x => x.Status == 1 && x.Id == healthData.Id)
                    .FirstOrDefaultAsync();
            if (existingHealthData is null) return false;
  
            //Todo
            existingHealthData.Height = healthData.Height;
            existingHealthData.Weight = healthData.Weight;
            existingHealthData.BloodType = healthData.BloodType;
            existingHealthData.Race = healthData.Race;
            existingHealthData.UseGlasses = healthData.UseGlasses;
            existingHealthData.UpdateDate = DateTime.UtcNow;    


            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} UpdateHealthData has generated an error", typeof(HealthDataRepository));

            return false;
        }
    }
}
