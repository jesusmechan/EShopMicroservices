using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Security.Application.Data;
public interface IApplicationDbContext 
{
    DbSet<Person> Persons { get; }
    DbSet<Role> Roles { get; }
    DbSet<User> Users { get; }
    DbSet<UserRole> UserRoles { get; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
