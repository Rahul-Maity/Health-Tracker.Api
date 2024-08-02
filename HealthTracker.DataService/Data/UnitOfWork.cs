using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.DataService.IConfiguration;
using HealthTracker.DataService.IRepository;
using HealthTracker.DataService.Repository;

using Microsoft.Extensions.Logging;

namespace HealthTracker.DataService.Data;
public class UnitOfWork:IUnitOfWork,IDisposable
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    public IUserRepository Users { get; private set; }

    public IRefreshTokenRepository RefreshTokens { get; private set; }

    public UnitOfWork(AppDbContext context,ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger("db_logs");
        Users = new UserRepository(_context, _logger);

        RefreshTokens = new RefreshTokenRepository(_context, _logger);
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
