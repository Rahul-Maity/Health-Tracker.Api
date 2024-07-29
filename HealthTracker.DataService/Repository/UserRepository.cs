﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.DataService.IRepository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthTracker.DataService.Repository;
public class UserRepository:GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context,ILogger logger):base(context, logger)
    {
        
    }

    public override async Task<IEnumerable<User>> All()
    {
        try
        {
            return await dbset.Where(x=>x.Status ==1)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "{Repo} all method has generated an error", typeof(UserRepository));

            return new List<User>();
        }
    }

}