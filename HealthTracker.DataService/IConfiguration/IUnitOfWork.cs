using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.DataService.IRepository;

namespace HealthTracker.DataService.IConfiguration;
public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task CompleteAsync();
}
