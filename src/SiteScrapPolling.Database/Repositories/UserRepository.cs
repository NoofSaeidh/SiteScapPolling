using Serilog;
using SiteScrapPolling.Common.Extensions;
using SiteScrapPolling.Database.Entities;

namespace SiteScrapPolling.Database.Repositories;

public interface IUserRepository
{
    Task<User> GetOrAddAsync(long id, CancellationToken cancellationToken);
    Task SetLastHandlerAsync(long id, string lastHandler, CancellationToken cancellationToken);
}

internal class UserRepository : IUserRepository
{
    private readonly DbContext _context;
    private readonly ILogger _logger;

    public UserRepository(DbContext context, ILogger logger)
    {
        _context = context;
        _logger  = logger;
    }


    public async Task<User> GetOrAddAsync(long id, CancellationToken cancellationToken)
    {
        return await GetOrAddOrUpdateAsync(id, null, cancellationToken);
    }

    public async Task SetLastHandlerAsync(long id, string lastHandler, CancellationToken cancellationToken)
    {
        await GetOrAddOrUpdateAsync(id, user => user.LastHandler = lastHandler, cancellationToken);
    }

    private async Task<User> GetOrAddOrUpdateAsync(long id, Action<User>? update, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
        _logger.Debug("User {Id} {FoundState}", id, (user != null).ToFoundState());
        if (user == null)
        {
            user = new User
            {
                Id       = id,
                LastDate = DateTimeOffset.UtcNow,
            };
            update?.Invoke(user);
            user = _context.Users.Add(user).Entity;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.Debug("User {Id} created", id);
        }
        else if (update != null)
        {
            update?.Invoke(user);
            user = _context.Users.Update(user).Entity;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.Debug("User {Id} updated", id);
        }

        return user;
    }
}
