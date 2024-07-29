using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker.DataService.IRepository;
public interface IGenericRepository<T>where T : class
{
    //Get all entities
    Task<IEnumerable<T>> All();

    //Get specific entity based on id
    Task<T> GetById(Guid id);

    Task<bool> Add(T entity);
    Task<bool> Delete(Guid id,string userId);

    //update entity or add if it doesn't exists
    Task<bool>Upsert(T entity);
}
