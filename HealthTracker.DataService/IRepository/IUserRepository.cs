using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker.DataService.IRepository;
public interface IUserRepository:IGenericRepository<User>
{
    Task<bool>UpdateUserProfile(User user);
    Task<User> GetByIdentityId(Guid identityId);
}
