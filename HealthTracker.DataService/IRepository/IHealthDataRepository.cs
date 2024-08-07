using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.Entities.DbSet;

namespace HealthTracker.DataService.IRepository;
public interface IHealthDataRepository:IGenericRepository<HealthData>
{


}
