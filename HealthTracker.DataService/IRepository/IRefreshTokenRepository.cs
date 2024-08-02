using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.Entities.DbSet;

namespace HealthTracker.DataService.IRepository;
public interface IRefreshTokenRepository:IGenericRepository<RefreshToken>
{

    Task<RefreshToken> GetByRefreshToken(string  refreshToken);

    Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken);
}
